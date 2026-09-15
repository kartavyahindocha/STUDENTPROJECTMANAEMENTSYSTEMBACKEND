using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Data;
using STUDENTPROJECTMANAEMENTSYSTEMBACKEND.DTOs.TaskPriorityMaster;
using STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Models;

namespace STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Controllers
{
    [Authorize(Roles = "Admin,Faculty,Student")]
    [Route("api/[controller]")]
    [ApiController]
    public class TaskPriorityMasterController : ControllerBase
    {
        private readonly AppDbContext context;
        private readonly IValidator<TaskPriorityMasterCreateEditDto> validator;

        #region DI
        public TaskPriorityMasterController(AppDbContext context, IValidator<TaskPriorityMasterCreateEditDto> validator)
        {
            this.context = context;
            this.validator = validator;
        }
        #endregion

        #region GetAllTaskPriorityMaster
        [HttpGet("/taskprioritymaster/list")]
        public async Task<IActionResult> GetAllTaskPriorityMaster()
        {
            var priorities = await context.TaskPriorities.AsNoTracking().ToListAsync();

            var dtoList = priorities.Select(p => new TaskPriorityMasterGetDto
            {
                TaskPriorityID      = p.TaskPriorityID,
                TaskPriorityName    = p.TaskPriorityName,
                TaskPriortyCssClass = p.TaskPriortyCssClass
            }).ToList();

            var response = ApiResponse.FromResponse(dtoList, "Task Priority list fetched successfully", "No Task Priorities found");
            return StatusCode(response.StatusCode, response);
        }
        #endregion

        #region CreateTaskPriorityMaster
        [Authorize(Roles = "Admin")]
        [HttpPost("/taskprioritymaster/create")]
        public async Task<IActionResult> CreateTaskPriorityMaster([FromBody] TaskPriorityMasterCreateEditDto taskPriority)
        {
            if (taskPriority == null)
            {
                var badRequestResponse = ApiResponse.BadRequest("INVALID DATA");
                return StatusCode(badRequestResponse.StatusCode, badRequestResponse);
            }

            var validationResult = await validator.ValidateAsync(taskPriority);
            if (!validationResult.IsValid)
            {
                var badRequestResponse = ApiResponse.BadRequest("Validation Failed", validationResult.Errors.Select(e => e.ErrorMessage));
                return StatusCode(badRequestResponse.StatusCode, badRequestResponse);
            }

            var taskPriorityToAdd = new TaskPriorityMaster
            {
                TaskPriorityName    = taskPriority.TaskPriorityName,
                TaskPriortyCssClass = taskPriority.TaskPriortyCssClass
            };

            await context.TaskPriorities.AddAsync(taskPriorityToAdd);
            await context.SaveChangesAsync();

            var response = ApiResponse.Created(taskPriorityToAdd, "Task Priority created successfully");
            return StatusCode(response.StatusCode, response);
        }
        #endregion

        #region GetTaskPriorityMasterById
        [HttpGet("/taskprioritymaster/getbyid/{id}")]
        public async Task<IActionResult> GetTaskPriorityMasterById(int id)
        {
            var priority = await context.TaskPriorities.FindAsync(id);
            if (priority == null)
            {
                var notFound = ApiResponse.NotFound("Task Priority Not Found");
                return StatusCode(notFound.StatusCode, notFound);
            }

            var dto = new TaskPriorityMasterGetDto
            {
                TaskPriorityID      = priority.TaskPriorityID,
                TaskPriorityName    = priority.TaskPriorityName,
                TaskPriortyCssClass = priority.TaskPriortyCssClass
            };

            var response = ApiResponse.Success(dto, "Task Priority fetched successfully");
            return StatusCode(response.StatusCode, response);
        }
        #endregion

        #region UpdateTaskPriorityMaster
        [Authorize(Roles = "Admin")]
        [HttpPut("/taskprioritymaster/update/{id}")]
        public async Task<IActionResult> UpdateTaskPriorityMaster(int id, [FromBody] TaskPriorityMasterCreateEditDto taskPriority)
        {
            if (taskPriority == null)
            {
                var badRequestResponse = ApiResponse.BadRequest("INVALID DATA");
                return StatusCode(badRequestResponse.StatusCode, badRequestResponse);
            }

            var validationResult = await validator.ValidateAsync(taskPriority);
            if (!validationResult.IsValid)
            {
                var badRequestResponse = ApiResponse.BadRequest("Validation Failed", validationResult.Errors.Select(e => e.ErrorMessage));
                return StatusCode(badRequestResponse.StatusCode, badRequestResponse);
            }

            var existingTaskPriority = await context.TaskPriorities.FindAsync(id);
            if (existingTaskPriority == null)
            {
                var notFoundResponse = ApiResponse.NotFound("Task Priority Not Found");
                return StatusCode(notFoundResponse.StatusCode, notFoundResponse);
            }

            existingTaskPriority.TaskPriorityName    = taskPriority.TaskPriorityName;
            existingTaskPriority.TaskPriortyCssClass = taskPriority.TaskPriortyCssClass;

            await context.SaveChangesAsync();

            var response = ApiResponse.Success(existingTaskPriority, "Task Priority updated successfully");
            return StatusCode(response.StatusCode, response);
        }
        #endregion

        #region DeleteTaskPriorityMasterByPK
        [Authorize(Roles = "Admin")]
        [HttpDelete("/taskprioritymaster/delete/{id}")]
        public async Task<IActionResult> DeleteTaskPriorityMasterByPK(int id)
        {
            var taskPriority = await context.TaskPriorities.FindAsync(id);
            if (taskPriority == null)
            {
                var notFoundResponse = ApiResponse.NotFound("Task Priority Not Found");
                return StatusCode(notFoundResponse.StatusCode, notFoundResponse);
            }

            context.TaskPriorities.Remove(taskPriority);
            await context.SaveChangesAsync();

            var response = ApiResponse.Success(taskPriority, "Task Priority deleted successfully");
            return StatusCode(response.StatusCode, response);
        }
        #endregion
    }
}
