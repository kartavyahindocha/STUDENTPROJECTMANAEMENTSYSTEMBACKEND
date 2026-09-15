using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Data;
using STUDENTPROJECTMANAEMENTSYSTEMBACKEND.DTOs.ProjectAllocation;
using STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Models;

namespace STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Controllers
{
    [Authorize(Roles = "Admin,Faculty,Student")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectAllocationController : ControllerBase
    {
        private readonly AppDbContext context;
        private readonly IValidator<ProjectAllocationCreateEditDto> validator;

        #region DI
        public ProjectAllocationController(AppDbContext context, IValidator<ProjectAllocationCreateEditDto> validator)
        {
            this.context = context;
            this.validator = validator;
        }
        #endregion

        #region GetAllProjectAllocation
        [HttpGet("/projectallocation/list")]
        public async Task<IActionResult> GetAllProjectAllocation()
        {
            var allocations = await context.ProjectAllocations
                .Include(pa => pa.ProjectMaster)
                .Include(pa => pa.Student)
                .Include(pa => pa.Faculty)
                .AsNoTracking()
                .ToListAsync();

            var dtoList = allocations.Select(pa => new ProjectAllocationGetDto
            {
                ProjectAllocationID  = pa.ProjectAllocationID,
                AssignedDate         = pa.AssignedDate,
                ProjectStartDate     = pa.ProjectStartDate,
                ProjectEndDate       = pa.ProjectEndDate,
                TotalTasksGiven      = pa.TotalTasksGiven,
                TotalCompletedTasks  = pa.TotalCompletedTasks,
                ProgressPercentage   = pa.ProgressPercentage,
                OverAllGrade         = pa.OverAllGrade,
                ProjectID            = pa.ProjectID,
                ProjectTitle         = pa.ProjectMaster?.ProjectTitle,
                StudentID            = pa.StudentID,
                StudentName          = pa.Student?.FullName,
                FacultyID            = pa.FacultyID,
                FacultyName          = pa.Faculty?.FullName
            }).ToList();

            var response = ApiResponse.FromResponse(dtoList, "ProjectAllocation list fetched successfully", "No ProjectAllocations found");
            return StatusCode(response.StatusCode, response);
        }
        #endregion

        #region CreateProjectAllocation
        [Authorize(Roles = "Admin,Faculty")]
        [HttpPost("/projectallocation/create")]
        public async Task<IActionResult> CreateProjectAllocation([FromBody] ProjectAllocationCreateEditDto allocation)
        {
            if (allocation == null)
            {
                var badRequestResponse = ApiResponse.BadRequest("INVALID DATA");
                return StatusCode(badRequestResponse.StatusCode, badRequestResponse);
            }

            var validationResult = await validator.ValidateAsync(allocation);
            if (!validationResult.IsValid)
            {
                var badRequestResponse = ApiResponse.BadRequest("Validation Failed", validationResult.Errors.Select(e => e.ErrorMessage));
                return StatusCode(badRequestResponse.StatusCode, badRequestResponse);
            }

            var allocationToAdd = new ProjectAllocation
            {
                AssignedDate        = allocation.AssignedDate,
                ProjectStartDate    = allocation.ProjectStartDate,
                ProjectEndDate      = allocation.ProjectEndDate,
                TotalTasksGiven     = allocation.TotalTasksGiven,
                TotalCompletedTasks = allocation.TotalCompletedTasks,
                ProgressPercentage  = allocation.ProgressPercentage,
                OverAllGrade        = allocation.OverAllGrade,
                ProjectID           = allocation.ProjectID,
                StudentID           = allocation.StudentID,
                FacultyID           = allocation.FacultyID
            };

            await context.ProjectAllocations.AddAsync(allocationToAdd);
            await context.SaveChangesAsync();

            var response = ApiResponse.Created(allocationToAdd, "ProjectAllocation created successfully");
            return StatusCode(response.StatusCode, response);
        }
        #endregion

        #region GetProjectAllocationById
        [HttpGet("/projectallocation/getbyid/{id}")]
        public async Task<IActionResult> GetProjectAllocationById(int id)
        {
            var pa = await context.ProjectAllocations
                .Include(pa => pa.ProjectMaster)
                .Include(pa => pa.Student)
                .Include(pa => pa.Faculty)
                .FirstOrDefaultAsync(pa => pa.ProjectAllocationID == id);

            if (pa == null)
            {
                var notFound = ApiResponse.NotFound("ProjectAllocation Not Found");
                return StatusCode(notFound.StatusCode, notFound);
            }

            var dto = new ProjectAllocationGetDto
            {
                ProjectAllocationID = pa.ProjectAllocationID,
                AssignedDate        = pa.AssignedDate,
                ProjectStartDate    = pa.ProjectStartDate,
                ProjectEndDate      = pa.ProjectEndDate,
                TotalTasksGiven     = pa.TotalTasksGiven,
                TotalCompletedTasks = pa.TotalCompletedTasks,
                ProgressPercentage  = pa.ProgressPercentage,
                OverAllGrade        = pa.OverAllGrade,
                ProjectID           = pa.ProjectID,
                ProjectTitle        = pa.ProjectMaster?.ProjectTitle,
                StudentID           = pa.StudentID,
                StudentName         = pa.Student?.FullName,
                FacultyID           = pa.FacultyID,
                FacultyName         = pa.Faculty?.FullName
            };

            var response = ApiResponse.Success(dto, "ProjectAllocation fetched successfully");
            return StatusCode(response.StatusCode, response);
        }
        #endregion

        #region UpdateProjectAllocation
        [Authorize(Roles = "Admin,Faculty")]
        [HttpPut("/projectallocation/update/{id}")]
        public async Task<IActionResult> UpdateProjectAllocation(int id, [FromBody] ProjectAllocationCreateEditDto allocation)
        {
            if (allocation == null)
            {
                var badRequestResponse = ApiResponse.BadRequest("INVALID DATA");
                return StatusCode(badRequestResponse.StatusCode, badRequestResponse);
            }

            var validationResult = await validator.ValidateAsync(allocation);
            if (!validationResult.IsValid)
            {
                var badRequestResponse = ApiResponse.BadRequest("Validation Failed", validationResult.Errors.Select(e => e.ErrorMessage));
                return StatusCode(badRequestResponse.StatusCode, badRequestResponse);
            }

            var existingAllocation = await context.ProjectAllocations.FindAsync(id);
            if (existingAllocation == null)
            {
                var notFoundResponse = ApiResponse.NotFound("ProjectAllocation Not Found");
                return StatusCode(notFoundResponse.StatusCode, notFoundResponse);
            }

            existingAllocation.AssignedDate        = allocation.AssignedDate;
            existingAllocation.ProjectStartDate    = allocation.ProjectStartDate;
            existingAllocation.ProjectEndDate      = allocation.ProjectEndDate;
            existingAllocation.TotalTasksGiven     = allocation.TotalTasksGiven;
            existingAllocation.TotalCompletedTasks = allocation.TotalCompletedTasks;
            existingAllocation.ProgressPercentage  = allocation.ProgressPercentage;
            existingAllocation.OverAllGrade        = allocation.OverAllGrade;
            existingAllocation.ProjectID           = allocation.ProjectID;
            existingAllocation.StudentID           = allocation.StudentID;
            existingAllocation.FacultyID           = allocation.FacultyID;

            await context.SaveChangesAsync();

            var response = ApiResponse.Success(existingAllocation, "ProjectAllocation updated successfully");
            return StatusCode(response.StatusCode, response);
        }
        #endregion

        #region DeleteProjectAllocationByPK
        [Authorize(Roles = "Admin,Faculty")]
        [HttpDelete("/projectallocation/delete/{id}")]
        public async Task<IActionResult> DeleteProjectAllocationByPK(int id)
        {
            var allocation = await context.ProjectAllocations.FindAsync(id);
            if (allocation == null)
            {
                var notFoundResponse = ApiResponse.NotFound("ProjectAllocation Not Found");
                return StatusCode(notFoundResponse.StatusCode, notFoundResponse);
            }

            context.ProjectAllocations.Remove(allocation);
            await context.SaveChangesAsync();

            var response = ApiResponse.Success(allocation, "ProjectAllocation deleted successfully");
            return StatusCode(response.StatusCode, response);
        }
        #endregion
    }
}
