using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Data;
using STUDENTPROJECTMANAEMENTSYSTEMBACKEND.DTOs.Tasks;
using STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Models;

namespace STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly AppDbContext context;
        private readonly IValidator<TasksCreateEditDto> createEditValidator;
        private readonly IValidator<TaskStudentUpdateDto> studentUpdateValidator;

        #region DI
        public TasksController(
            AppDbContext context,
            IValidator<TasksCreateEditDto> createEditValidator,
            IValidator<TaskStudentUpdateDto> studentUpdateValidator)
        {
            this.context = context;
            this.createEditValidator = createEditValidator;
            this.studentUpdateValidator = studentUpdateValidator;
        }
        #endregion

        #region GetAllTasks
        [HttpGet("/tasks/list")]
        public async Task<IActionResult> GetAllTasks()
        {
            var tasks = await context.Tasks
                .Include(t => t.TaskStatus)
                .Include(t => t.TaskPriority)
                .Include(t => t.ProjectAllocation)
                    .ThenInclude(pa => pa.ProjectMaster)
                .Include(t => t.ProjectAllocation)
                    .ThenInclude(pa => pa.Student)
                .Include(t => t.ProjectAllocation)
                    .ThenInclude(pa => pa.Faculty)
                .AsNoTracking()
                .ToListAsync();

            var dtoList = tasks.Select(t => new TasksGetDto
            {
                TaskID              = t.TaskID,
                TaskTitle           = t.TaskTitle,
                TaskDescription     = t.TaskDescription,
                AssignedScore       = t.AssignedScore,
                EarnedScore         = t.EarnedScore,
                ProgressPercentage  = t.ProgressPercentage,
                TaskAssignedDate    = t.TaskAssignedDate,
                TaskStartDate       = t.TaskStartDate,
                TaskDueDate         = t.TaskDueDate,
                TaskCompletedDate   = t.TaskCompletedDate,
                NextFollowUpDate    = t.NextFollowUpDate,
                FacultyRemarks      = t.FacultyRemarks,
                StudentRemarks      = t.StudentRemarks,
                ProjectAllocationID = t.ProjectAllocationID,
                ProjectTitle        = t.ProjectAllocation?.ProjectMaster?.ProjectTitle,
                ProjectDescription  = t.ProjectAllocation?.ProjectMaster?.Description,
                StudentName         = t.ProjectAllocation?.Student?.FullName,
                FacultyName         = t.ProjectAllocation?.Faculty?.FullName,
                TaskStatusID        = t.TaskStatusID,
                TaskStatusName      = t.TaskStatus?.TaskStatusName,
                TaskPriorityID      = t.TaskPriorityID,
                TaskPriorityName    = t.TaskPriority?.TaskPriorityName
            }).ToList();

            var response = ApiResponse.FromResponse(dtoList, "Task list fetched successfully", "No tasks found");
            return StatusCode(response.StatusCode, response);
        }
        #endregion

        #region CreateTasks
        [HttpPost("/tasks/create")]
        public async Task<IActionResult> CreateTasks([FromBody] TasksCreateEditDto task)
        {
            if (task == null)
            {
                var badRequestResponse = ApiResponse.BadRequest("INVALID DATA");
                return StatusCode(badRequestResponse.StatusCode, badRequestResponse);
            }

            var validationResult = await createEditValidator.ValidateAsync(task);
            if (!validationResult.IsValid)
            {
                var badRequestResponse = ApiResponse.BadRequest("Validation Failed", validationResult.Errors.Select(e => e.ErrorMessage));
                return StatusCode(badRequestResponse.StatusCode, badRequestResponse);
            }

            var taskToAdd = new Tasks
            {
                TaskTitle           = task.TaskTitle,
                TaskDescription     = task.TaskDescription,
                AssignedScore       = task.AssignedScore,
                EarnedScore         = task.EarnedScore,
                ProgressPercentage  = task.ProgressPercentage,
                TaskAssignedDate    = task.TaskAssignedDate,
                TaskStartDate       = task.TaskStartDate,
                TaskDueDate         = task.TaskDueDate,
                TaskCompletedDate   = task.TaskCompletedDate,
                NextFollowUpDate    = task.NextFollowUpDate,
                FacultyRemarks      = task.FacultyRemarks,
                StudentRemarks      = task.StudentRemarks,
                ProjectAllocationID = task.ProjectAllocationID,
                TaskStatusID        = task.TaskStatusID,
                TaskPriorityID      = task.TaskPriorityID
            };

            await context.Tasks.AddAsync(taskToAdd);
            await context.SaveChangesAsync();

            var response = ApiResponse.Created(taskToAdd, "Task created successfully");
            return StatusCode(response.StatusCode, response);
        }
        #endregion

        #region GetTasksById
        [HttpGet("/tasks/getbyid/{id}")]
        public async Task<IActionResult> GetTasksById(int id)
        {
            var t = await context.Tasks
                .Include(t => t.TaskStatus)
                .Include(t => t.TaskPriority)
                .Include(t => t.ProjectAllocation)
                    .ThenInclude(pa => pa.ProjectMaster)
                .Include(t => t.ProjectAllocation)
                    .ThenInclude(pa => pa.Student)
                .Include(t => t.ProjectAllocation)
                    .ThenInclude(pa => pa.Faculty)
                .FirstOrDefaultAsync(t => t.TaskID == id);

            if (t == null)
            {
                var notFound = ApiResponse.NotFound("Task Not Found");
                return StatusCode(notFound.StatusCode, notFound);
            }

            var dto = new TasksGetDto
            {
                TaskID              = t.TaskID,
                TaskTitle           = t.TaskTitle,
                TaskDescription     = t.TaskDescription,
                AssignedScore       = t.AssignedScore,
                EarnedScore         = t.EarnedScore,
                ProgressPercentage  = t.ProgressPercentage,
                TaskAssignedDate    = t.TaskAssignedDate,
                TaskStartDate       = t.TaskStartDate,
                TaskDueDate         = t.TaskDueDate,
                TaskCompletedDate   = t.TaskCompletedDate,
                NextFollowUpDate    = t.NextFollowUpDate,
                FacultyRemarks      = t.FacultyRemarks,
                StudentRemarks      = t.StudentRemarks,
                ProjectAllocationID = t.ProjectAllocationID,
                ProjectTitle        = t.ProjectAllocation?.ProjectMaster?.ProjectTitle,
                ProjectDescription  = t.ProjectAllocation?.ProjectMaster?.Description,
                StudentName         = t.ProjectAllocation?.Student?.FullName,
                FacultyName         = t.ProjectAllocation?.Faculty?.FullName,
                TaskStatusID        = t.TaskStatusID,
                TaskStatusName      = t.TaskStatus?.TaskStatusName,
                TaskPriorityID      = t.TaskPriorityID,
                TaskPriorityName    = t.TaskPriority?.TaskPriorityName
            };

            var response = ApiResponse.Success(dto, "Task fetched successfully");
            return StatusCode(response.StatusCode, response);
        }
        #endregion

        #region UpdateTasks
        [HttpPut("/tasks/update/{id}")]
        public async Task<IActionResult> UpdateTasks(int id, [FromBody] TasksCreateEditDto task)
        {
            if (task == null)
            {
                var badRequestResponse = ApiResponse.BadRequest("INVALID DATA");
                return StatusCode(badRequestResponse.StatusCode, badRequestResponse);
            }

            var validationResult = await createEditValidator.ValidateAsync(task);
            if (!validationResult.IsValid)
            {
                var badRequestResponse = ApiResponse.BadRequest("Validation Failed", validationResult.Errors.Select(e => e.ErrorMessage));
                return StatusCode(badRequestResponse.StatusCode, badRequestResponse);
            }

            var existingTask = await context.Tasks.FindAsync(id);
            if (existingTask == null)
            {
                var notFoundResponse = ApiResponse.NotFound("Task Not Found");
                return StatusCode(notFoundResponse.StatusCode, notFoundResponse);
            }

            existingTask.TaskTitle           = task.TaskTitle;
            existingTask.TaskDescription     = task.TaskDescription;
            existingTask.AssignedScore       = task.AssignedScore;
            existingTask.EarnedScore         = task.EarnedScore;
            existingTask.ProgressPercentage  = task.ProgressPercentage;
            existingTask.TaskAssignedDate    = task.TaskAssignedDate;
            existingTask.TaskStartDate       = task.TaskStartDate;
            existingTask.TaskDueDate         = task.TaskDueDate;
            existingTask.TaskCompletedDate   = task.TaskCompletedDate;
            existingTask.NextFollowUpDate    = task.NextFollowUpDate;
            existingTask.FacultyRemarks      = task.FacultyRemarks;
            existingTask.StudentRemarks      = task.StudentRemarks;
            existingTask.ProjectAllocationID = task.ProjectAllocationID;
            existingTask.TaskStatusID        = task.TaskStatusID;
            existingTask.TaskPriorityID      = task.TaskPriorityID;

            await context.SaveChangesAsync();

            var response = ApiResponse.Success(existingTask, "Task updated successfully");
            return StatusCode(response.StatusCode, response);
        }
        #endregion

        #region StudentProgressUpdate (For Student Task Update Form)
        [HttpPut("/tasks/studentupdate/{id}")]
        public async Task<IActionResult> StudentProgressUpdate(int id, [FromBody] TaskStudentUpdateDto dto)
        {
            if (dto == null)
            {
                var badRequestResponse = ApiResponse.BadRequest("INVALID DATA");
                return StatusCode(badRequestResponse.StatusCode, badRequestResponse);
            }

            var validationResult = await studentUpdateValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                var badRequestResponse = ApiResponse.BadRequest("Validation Failed", validationResult.Errors.Select(e => e.ErrorMessage));
                return StatusCode(badRequestResponse.StatusCode, badRequestResponse);
            }

            var existingTask = await context.Tasks.FindAsync(id);
            if (existingTask == null)
            {
                var notFoundResponse = ApiResponse.NotFound("Task Not Found");
                return StatusCode(notFoundResponse.StatusCode, notFoundResponse);
            }

            existingTask.ProgressPercentage = dto.ProgressPercentage;
            existingTask.StudentRemarks = dto.StudentRemarks;

            if (dto.TaskStatusID > 0)
            {
                existingTask.TaskStatusID = dto.TaskStatusID;
            }

            if (dto.ProgressPercentage >= 100m && existingTask.TaskCompletedDate == null)
            {
                existingTask.TaskCompletedDate = DateTime.Now;
            }

            if (existingTask.TaskStartDate == null && dto.ProgressPercentage > 0)
            {
                existingTask.TaskStartDate = DateTime.Now;
            }

            await context.SaveChangesAsync();

            var response = ApiResponse.Success(existingTask, "Student task progress updated successfully");
            return StatusCode(response.StatusCode, response);
        }
        #endregion

        #region DeleteTasksByPK
        [HttpDelete("/tasks/delete/{id}")]
        public async Task<IActionResult> DeleteTasksByPK(int id)
        {
            var task = await context.Tasks.FindAsync(id);
            if (task == null)
            {
                var notFoundResponse = ApiResponse.NotFound("Task Not Found");
                return StatusCode(notFoundResponse.StatusCode, notFoundResponse);
            }

            context.Tasks.Remove(task);
            await context.SaveChangesAsync();

            var response = ApiResponse.Success(task, "Task deleted successfully");
            return StatusCode(response.StatusCode, response);
        }
        #endregion
    }
}
