using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Data;
using STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Models;

namespace STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectMasterController : ControllerBase
    {
        private readonly AppDbContext context;

        #region DI
        public ProjectMasterController(AppDbContext context)
        {
            this.context = context;
        }
        #endregion

        #region GetAllProjectMaster
        [HttpGet("/projectmaster/list")]
        public async Task<IActionResult> GetAllProjectMaster()
        {
            var projectMaster = await context.ProjectMasters
                                             .AsNoTracking()
                                             .ToListAsync();

            return Ok(projectMaster);
        }
        #endregion

        #region CreateProjectMaster
        [HttpPost("/projectmaster/create")]
        public async Task<IActionResult> CreateProjectMaster([FromBody] ProjectMasterDTO project)
        {
            if (project == null)
            {
                return BadRequest("INVALID DATA");
            }

            var projectToAdd = new ProjectMaster
            {
                ProjectTitle = project.ProjectTitle,
                Description = project.Description
            };

            await context.ProjectMasters.AddAsync(projectToAdd);
            await context.SaveChangesAsync();

            return Ok(projectToAdd);
        }
        #endregion

        #region GetProjectMasterById
        [HttpGet("/projectmaster/getbyid/{id}")]
        public async Task<IActionResult> GetProjectMasterById(int id)
        {
            var project = await context.ProjectMasters.FindAsync(id);

            if (project == null)
            {
                return NotFound("Project Not Found");
            }

            return Ok(project);
        }
        #endregion

        #region DeleteProjectMaster
        [HttpDelete("/projectmaster/delete/{id}")]
        public async Task<IActionResult> DeleteProjectMaster(int id)
        {
            var project = await context.ProjectMasters.FindAsync(id);

            if (project == null)
            {
                return NotFound("Project Not Found");
            }

            context.ProjectMasters.Remove(project);
            await context.SaveChangesAsync();

            return Ok(project);
        }
        #endregion
    }
}