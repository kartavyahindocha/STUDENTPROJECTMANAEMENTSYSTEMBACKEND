using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Data;

namespace STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectAllocationController : ControllerBase
    {
        private readonly AppDbContext context;
        #region DI
        public ProjectAllocationController(AppDbContext context)
        {
            this.context = context;
        }
        #endregion

        #region GetAllProjectAllocation
        [HttpGet("/projectallocation/list")]
        public async Task<IActionResult> GetAllProjectAllocation()
        {
            var projectAllocation = await context.ProjectAllocations.ToListAsync();
            return Ok(projectAllocation);
        }
        #endregion
    }
}
