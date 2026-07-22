using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Data;
using STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Models;

namespace STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskStatusMasterController : ControllerBase
    {
        private readonly AppDbContext context;

        #region DI
        public TaskStatusMasterController(AppDbContext context)
        {
            this.context = context;
        }
        #endregion

        #region GetAllTaskStatusMaster
        [HttpGet("/taskstatusmaster/list")]
        public async Task<IActionResult> GetAllTaskStatusMaster()
        {
            var taskStatusMaster = await context.TaskStatus
                                                .AsNoTracking()
                                                .ToListAsync();

            return Ok(taskStatusMaster);
        }
        #endregion

        #region CreateTaskStatusMaster
        [HttpPost("/taskstatusmaster/create")]
        public async Task<IActionResult> CreateTaskStatusMaster([FromBody] TaskStatusMasterDTO taskStatus)
        {
            if (taskStatus == null)
            {
                return BadRequest("INVALID DATA");
            }

            var taskStatusToAdd = new TaskStatusMaster
            {
                TaskStatusName = taskStatus.TaskStatusName,
                TaskStatusCssClass = taskStatus.TaskStatusCssClass
            };

            await context.TaskStatus.AddAsync(taskStatusToAdd);
            await context.SaveChangesAsync();

            return Ok(taskStatusToAdd);
        }
        #endregion

        #region GetTaskStatusMasterById
        [HttpGet("/taskstatusmaster/getbyid/{id}")]
        public async Task<IActionResult> GetTaskStatusMasterById(int id)
        {
            var taskStatus = await context.TaskStatus.FindAsync(id);

            if (taskStatus == null)
            {
                return NotFound("Task Status Not Found");
            }

            return Ok(taskStatus);
        }
        #endregion

        #region UpdateTaskStatusMaster
        [HttpPut("/taskstatusmaster/update/{id}")]
        public async Task<IActionResult> UpdateTaskStatusMaster(int id, [FromBody] TaskStatusMasterDTO taskStatus)
        {
            if (taskStatus == null)
            {
                return BadRequest("INVALID DATA");
            }

            var existingTaskStatus = await context.TaskStatus.FindAsync(id);

            if (existingTaskStatus == null)
            {
                return NotFound("Task Status Not Found");
            }

            existingTaskStatus.TaskStatusName = taskStatus.TaskStatusName;
            existingTaskStatus.TaskStatusCssClass = taskStatus.TaskStatusCssClass;

            await context.SaveChangesAsync();

            return Ok(existingTaskStatus);
        }
        #endregion

        #region DeleteTaskStatusMaster
        [HttpDelete("/taskstatusmaster/delete/{id}")]
        public async Task<IActionResult> DeleteTaskStatusMaster(int id)
        {
            var taskStatus = await context.TaskStatus.FindAsync(id);

            if (taskStatus == null)
            {
                return NotFound("Task Status Not Found");
            }

            context.TaskStatus.Remove(taskStatus);
            await context.SaveChangesAsync();

            return Ok(taskStatus);
        }
        #endregion
    }
}