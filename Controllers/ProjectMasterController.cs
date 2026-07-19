using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Data;

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
            var projectMaster = await context.ProjectMasters.ToListAsync();
            return Ok(projectMaster);
        }
        #endregion
    }
}
