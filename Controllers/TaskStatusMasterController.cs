using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Data;
using STUDENTPROJECTMANAEMENTSYSTEMBACKEND.DTOs.TaskStatusMaster;
using STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Models;

namespace STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskStatusMasterController : ControllerBase
    {
        private readonly AppDbContext context;
        private readonly IValidator<TaskStatusMasterCreateEditDto> validator;

        #region DI
        public TaskStatusMasterController(AppDbContext context, IValidator<TaskStatusMasterCreateEditDto> validator)
        {
            this.context = context;
            this.validator = validator;
        }
        #endregion

        #region GetAllTaskStatusMaster
        [HttpGet("/taskstatusmaster/list")]
        public async Task<IActionResult> GetAllTaskStatusMaster()
        {
            var statuses = await context.TaskStatus.AsNoTracking().ToListAsync();

            var dtoList = statuses.Select(s => new TaskStatusMasterGetDto
            {
                TaskStatusID       = s.TaskStatusID,
                TaskStatusName     = s.TaskStatusName,
                TaskStatusCssClass = s.TaskStatusCssClass
            }).ToList();

            var response = ApiResponse.FromResponse(dtoList, "Task Status list fetched successfully", "No Task Statuses found");
            return StatusCode(response.StatusCode, response);
        }
        #endregion

        #region CreateTaskStatusMaster
        [HttpPost("/taskstatusmaster/create")]
        public async Task<IActionResult> CreateTaskStatusMaster([FromBody] TaskStatusMasterCreateEditDto taskStatus)
        {
            if (taskStatus == null)
            {
                var badRequestResponse = ApiResponse.BadRequest("INVALID DATA");
                return StatusCode(badRequestResponse.StatusCode, badRequestResponse);
            }

            var validationResult = await validator.ValidateAsync(taskStatus);
            if (!validationResult.IsValid)
            {
                var badRequestResponse = ApiResponse.BadRequest("Validation Failed", validationResult.Errors.Select(e => e.ErrorMessage));
                return StatusCode(badRequestResponse.StatusCode, badRequestResponse);
            }

            var taskStatusToAdd = new TaskStatusMaster
            {
                TaskStatusName     = taskStatus.TaskStatusName,
                TaskStatusCssClass = taskStatus.TaskStatusCssClass
            };

            await context.TaskStatus.AddAsync(taskStatusToAdd);
            await context.SaveChangesAsync();

            var response = ApiResponse.Created(taskStatusToAdd, "Task Status created successfully");
            return StatusCode(response.StatusCode, response);
        }
        #endregion

        #region GetTaskStatusMasterById
        [HttpGet("/taskstatusmaster/getbyid/{id}")]
        public async Task<IActionResult> GetTaskStatusMasterById(int id)
        {
            var status = await context.TaskStatus.FindAsync(id);
            if (status == null)
            {
                var notFound = ApiResponse.NotFound("Task Status Not Found");
                return StatusCode(notFound.StatusCode, notFound);
            }

            var dto = new TaskStatusMasterGetDto
            {
                TaskStatusID       = status.TaskStatusID,
                TaskStatusName     = status.TaskStatusName,
                TaskStatusCssClass = status.TaskStatusCssClass
            };

            var response = ApiResponse.Success(dto, "Task Status fetched successfully");
            return StatusCode(response.StatusCode, response);
        }
        #endregion

        #region UpdateTaskStatusMaster
        [HttpPut("/taskstatusmaster/update/{id}")]
        public async Task<IActionResult> UpdateTaskStatusMaster(int id, [FromBody] TaskStatusMasterCreateEditDto taskStatus)
        {
            if (taskStatus == null)
            {
                var badRequestResponse = ApiResponse.BadRequest("INVALID DATA");
                return StatusCode(badRequestResponse.StatusCode, badRequestResponse);
            }

            var validationResult = await validator.ValidateAsync(taskStatus);
            if (!validationResult.IsValid)
            {
                var badRequestResponse = ApiResponse.BadRequest("Validation Failed", validationResult.Errors.Select(e => e.ErrorMessage));
                return StatusCode(badRequestResponse.StatusCode, badRequestResponse);
            }

            var existingTaskStatus = await context.TaskStatus.FindAsync(id);
            if (existingTaskStatus == null)
            {
                var notFoundResponse = ApiResponse.NotFound("Task Status Not Found");
                return StatusCode(notFoundResponse.StatusCode, notFoundResponse);
            }

            existingTaskStatus.TaskStatusName     = taskStatus.TaskStatusName;
            existingTaskStatus.TaskStatusCssClass = taskStatus.TaskStatusCssClass;

            await context.SaveChangesAsync();

            var response = ApiResponse.Success(existingTaskStatus, "Task Status updated successfully");
            return StatusCode(response.StatusCode, response);
        }
        #endregion

        #region DeleteTaskStatusMasterByPK
        [HttpDelete("/taskstatusmaster/delete/{id}")]
        public async Task<IActionResult> DeleteTaskStatusMasterByPK(int id)
        {
            var taskStatus = await context.TaskStatus.FindAsync(id);
            if (taskStatus == null)
            {
                var notFoundResponse = ApiResponse.NotFound("Task Status Not Found");
                return StatusCode(notFoundResponse.StatusCode, notFoundResponse);
            }

            context.TaskStatus.Remove(taskStatus);
            await context.SaveChangesAsync();

            var response = ApiResponse.Success(taskStatus, "Task Status deleted successfully");
            return StatusCode(response.StatusCode, response);
        }
        #endregion
    }
}