using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Data;
using STUDENTPROJECTMANAEMENTSYSTEMBACKEND.DTOs.ProjectMaster;
using STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Models;

namespace STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectMasterController : ControllerBase
    {
        private readonly AppDbContext context;
        private readonly IValidator<ProjectMasterCreateEditDto> validator;

        #region DI
        public ProjectMasterController(AppDbContext context, IValidator<ProjectMasterCreateEditDto> validator)
        {
            this.context = context;
            this.validator = validator;
        }
        #endregion

        #region GetAllProjectMaster
        [HttpGet("/projectmaster/list")]
        public async Task<IActionResult> GetAllProjectMaster()
        {
            var projects = await context.ProjectMasters
                .Include(p => p.ProjectAllocations)
                    .ThenInclude(pa => pa.Faculty)
                .Include(p => p.ProjectAllocations)
                    .ThenInclude(pa => pa.Student)
                .AsNoTracking()
                .ToListAsync();

            var dtoList = projects.Select(p =>
            {
                var alloc = p.ProjectAllocations.FirstOrDefault();
                return new ProjectMasterGetDto
                {
                    ProjectID    = p.ProjectID,
                    ProjectTitle = p.ProjectTitle,
                    Description  = p.Description,
                    FacultyID    = alloc?.FacultyID,
                    FacultyName  = alloc?.Faculty?.FullName,
                    StudentID    = alloc?.StudentID,
                    StudentName  = alloc?.Student?.FullName
                };
            }).ToList();

            var response = ApiResponse.FromResponse(dtoList, "Project list fetched successfully", "No projects found");
            return StatusCode(response.StatusCode, response);
        }
        #endregion

        #region CreateProjectMaster
        [HttpPost("/projectmaster/create")]
        public async Task<IActionResult> CreateProjectMaster([FromBody] ProjectMasterCreateEditDto project)
        {
            if (project == null)
            {
                var badRequestResponse = ApiResponse.BadRequest("INVALID DATA");
                return StatusCode(badRequestResponse.StatusCode, badRequestResponse);
            }

            var validationResult = await validator.ValidateAsync(project);
            if (!validationResult.IsValid)
            {
                var badRequestResponse = ApiResponse.BadRequest("Validation Failed", validationResult.Errors.Select(e => e.ErrorMessage));
                return StatusCode(badRequestResponse.StatusCode, badRequestResponse);
            }

            var projectToAdd = new ProjectMaster
            {
                ProjectTitle = project.ProjectTitle,
                Description  = project.Description
            };

            await context.ProjectMasters.AddAsync(projectToAdd);
            await context.SaveChangesAsync();

            // If FacultyID or StudentID is provided, create a ProjectAllocation record
            if (project.FacultyID.HasValue || project.StudentID.HasValue)
            {
                var allocation = new ProjectAllocation
                {
                    ProjectID          = projectToAdd.ProjectID,
                    FacultyID          = project.FacultyID ?? 1,
                    StudentID          = project.StudentID ?? 1,
                    AssignedDate       = DateTime.Now,
                    ProjectStartDate   = DateTime.Now,
                    ProjectEndDate     = DateTime.Now.AddDays(90),
                    TotalTasksGiven    = 0,
                    TotalCompletedTasks = 0,
                    ProgressPercentage = 0m
                };
                await context.ProjectAllocations.AddAsync(allocation);
                await context.SaveChangesAsync();
            }

            var response = ApiResponse.Created(projectToAdd, "Project created successfully");
            return StatusCode(response.StatusCode, response);
        }
        #endregion

        #region GetProjectMasterById
        [HttpGet("/projectmaster/getbyid/{id}")]
        public async Task<IActionResult> GetProjectMasterById(int id)
        {
            var project = await context.ProjectMasters
                .Include(p => p.ProjectAllocations)
                    .ThenInclude(pa => pa.Faculty)
                .Include(p => p.ProjectAllocations)
                    .ThenInclude(pa => pa.Student)
                .FirstOrDefaultAsync(p => p.ProjectID == id);

            if (project == null)
            {
                var notFound = ApiResponse.NotFound("Project Not Found");
                return StatusCode(notFound.StatusCode, notFound);
            }

            var alloc = project.ProjectAllocations.FirstOrDefault();

            var dto = new ProjectMasterGetDto
            {
                ProjectID    = project.ProjectID,
                ProjectTitle = project.ProjectTitle,
                Description  = project.Description,
                FacultyID    = alloc?.FacultyID,
                FacultyName  = alloc?.Faculty?.FullName,
                StudentID    = alloc?.StudentID,
                StudentName  = alloc?.Student?.FullName
            };

            var response = ApiResponse.Success(dto, "Project fetched successfully");
            return StatusCode(response.StatusCode, response);
        }
        #endregion

        #region UpdateProjectMaster
        [HttpPut("/projectmaster/update/{id}")]
        public async Task<IActionResult> UpdateProjectMaster(int id, [FromBody] ProjectMasterCreateEditDto project)
        {
            if (project == null)
            {
                var badRequestResponse = ApiResponse.BadRequest("INVALID DATA");
                return StatusCode(badRequestResponse.StatusCode, badRequestResponse);
            }

            var validationResult = await validator.ValidateAsync(project);
            if (!validationResult.IsValid)
            {
                var badRequestResponse = ApiResponse.BadRequest("Validation Failed", validationResult.Errors.Select(e => e.ErrorMessage));
                return StatusCode(badRequestResponse.StatusCode, badRequestResponse);
            }

            var existingProject = await context.ProjectMasters
                .Include(p => p.ProjectAllocations)
                .FirstOrDefaultAsync(p => p.ProjectID == id);

            if (existingProject == null)
            {
                var notFoundResponse = ApiResponse.NotFound("Project Not Found");
                return StatusCode(notFoundResponse.StatusCode, notFoundResponse);
            }

            existingProject.ProjectTitle = project.ProjectTitle;
            existingProject.Description  = project.Description;

            // Handle Allocation sync
            if (project.FacultyID.HasValue || project.StudentID.HasValue)
            {
                var existingAlloc = existingProject.ProjectAllocations.FirstOrDefault();
                if (existingAlloc != null)
                {
                    if (project.FacultyID.HasValue) existingAlloc.FacultyID = project.FacultyID.Value;
                    if (project.StudentID.HasValue) existingAlloc.StudentID = project.StudentID.Value;
                }
                else
                {
                    var newAlloc = new ProjectAllocation
                    {
                        ProjectID           = existingProject.ProjectID,
                        FacultyID           = project.FacultyID ?? 1,
                        StudentID           = project.StudentID ?? 1,
                        AssignedDate        = DateTime.Now,
                        ProjectStartDate    = DateTime.Now,
                        ProjectEndDate      = DateTime.Now.AddDays(90),
                        TotalTasksGiven     = 0,
                        TotalCompletedTasks  = 0,
                        ProgressPercentage  = 0m
                    };
                    await context.ProjectAllocations.AddAsync(newAlloc);
                }
            }

            await context.SaveChangesAsync();

            var response = ApiResponse.Success(existingProject, "Project updated successfully");
            return StatusCode(response.StatusCode, response);
        }
        #endregion

        #region DeleteProjectMasterByPK
        [HttpDelete("/projectmaster/delete/{id}")]
        public async Task<IActionResult> DeleteProjectMasterByPK(int id)
        {
            var project = await context.ProjectMasters.FindAsync(id);
            if (project == null)
            {
                var notFoundResponse = ApiResponse.NotFound("Project Not Found");
                return StatusCode(notFoundResponse.StatusCode, notFoundResponse);
            }

            context.ProjectMasters.Remove(project);
            await context.SaveChangesAsync();

            var response = ApiResponse.Success(project, "Project deleted successfully");
            return StatusCode(response.StatusCode, response);
        }
        #endregion
    }
}