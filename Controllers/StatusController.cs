using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Data;
using STUDENTPROJECTMANAEMENTSYSTEMBACKEND.DTOs.TaskStatusMaster;
using STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Models;

namespace STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Controllers
{
    [Authorize(Roles = "Admin,Faculty,Student")]
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        private readonly AppDbContext context;
        private readonly IValidator<TaskStatusMasterCreateEditDto> validator;

        #region DI
        public StatusController(AppDbContext context, IValidator<TaskStatusMasterCreateEditDto> validator)
        {
            this.context = context;
            this.validator = validator;
        }
        #endregion

        #region GetAllStatus
        [HttpGet("/status/list")]
        public async Task<IActionResult> GetAllStatus()
        {
            var statuses = await context.TaskStatus.AsNoTracking().ToListAsync();

            var dtoList = statuses.Select(s => new TaskStatusMasterGetDto
            {
                TaskStatusID       = s.TaskStatusID,
                TaskStatusName     = s.TaskStatusName,
                TaskStatusCssClass = s.TaskStatusCssClass
            }).ToList();

            var response = ApiResponse.FromResponse(dtoList, "Status list fetched successfully", "No statuses found");
            return StatusCode(response.StatusCode, response);
        }
        #endregion

        #region CreateStatus
        [Authorize(Roles = "Admin")]
        [HttpPost("/status/create")]
        public async Task<IActionResult> CreateStatus([FromBody] TaskStatusMasterCreateEditDto status)
        {
            if (status == null)
            {
                var badRequestResponse = ApiResponse.BadRequest("INVALID DATA");
                return StatusCode(badRequestResponse.StatusCode, badRequestResponse);
            }

            var validationResult = await validator.ValidateAsync(status);
            if (!validationResult.IsValid)
            {
                var badRequestResponse = ApiResponse.BadRequest("Validation Failed", validationResult.Errors.Select(e => e.ErrorMessage));
                return StatusCode(badRequestResponse.StatusCode, badRequestResponse);
            }

            var statusToAdd = new TaskStatusMaster
            {
                TaskStatusName     = status.TaskStatusName,
                TaskStatusCssClass = status.TaskStatusCssClass
            };

            await context.TaskStatus.AddAsync(statusToAdd);
            await context.SaveChangesAsync();

            var response = ApiResponse.Created(statusToAdd, "Status created successfully");
            return StatusCode(response.StatusCode, response);
        }
        #endregion

        #region GetStatusById
        [HttpGet("/status/getbyid/{id}")]
        public async Task<IActionResult> GetStatusById(int id)
        {
            var status = await context.TaskStatus.FindAsync(id);
            if (status == null)
            {
                var notFound = ApiResponse.NotFound("Status Not Found");
                return StatusCode(notFound.StatusCode, notFound);
            }

            var dto = new TaskStatusMasterGetDto
            {
                TaskStatusID       = status.TaskStatusID,
                TaskStatusName     = status.TaskStatusName,
                TaskStatusCssClass = status.TaskStatusCssClass
            };

            var response = ApiResponse.Success(dto, "Status fetched successfully");
            return StatusCode(response.StatusCode, response);
        }
        #endregion

        #region UpdateStatus
        [Authorize(Roles = "Admin")]
        [HttpPut("/status/update/{id}")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] TaskStatusMasterCreateEditDto status)
        {
            if (status == null)
            {
                var badRequestResponse = ApiResponse.BadRequest("INVALID DATA");
                return StatusCode(badRequestResponse.StatusCode, badRequestResponse);
            }

            var validationResult = await validator.ValidateAsync(status);
            if (!validationResult.IsValid)
            {
                var badRequestResponse = ApiResponse.BadRequest("Validation Failed", validationResult.Errors.Select(e => e.ErrorMessage));
                return StatusCode(badRequestResponse.StatusCode, badRequestResponse);
            }

            var existingStatus = await context.TaskStatus.FindAsync(id);
            if (existingStatus == null)
            {
                var notFoundResponse = ApiResponse.NotFound("Status Not Found");
                return StatusCode(notFoundResponse.StatusCode, notFoundResponse);
            }

            existingStatus.TaskStatusName     = status.TaskStatusName;
            existingStatus.TaskStatusCssClass = status.TaskStatusCssClass;

            await context.SaveChangesAsync();

            var response = ApiResponse.Success(existingStatus, "Status updated successfully");
            return StatusCode(response.StatusCode, response);
        }
        #endregion

        #region DeleteStatusByPK
        [Authorize(Roles = "Admin")]
        [HttpDelete("/status/delete/{id}")]
        public async Task<IActionResult> DeleteStatusByPK(int id)
        {
            var status = await context.TaskStatus.FindAsync(id);
            if (status == null)
            {
                var notFoundResponse = ApiResponse.NotFound("Status Not Found");
                return StatusCode(notFoundResponse.StatusCode, notFoundResponse);
            }

            context.TaskStatus.Remove(status);
            await context.SaveChangesAsync();

            var response = ApiResponse.Success(status, "Status deleted successfully");
            return StatusCode(response.StatusCode, response);
        }
        #endregion
    }
}
