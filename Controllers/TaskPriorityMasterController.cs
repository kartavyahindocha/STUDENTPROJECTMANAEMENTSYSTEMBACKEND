using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Data;

namespace STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskPriorityMasterController : ControllerBase
    {
        private readonly AppDbContext context;
        #region DI
        public TaskPriorityMasterController(AppDbContext context)
        {
            this.context = context;
        }
        #endregion

        #region GetAllTaskPriorityMaster
        [HttpGet("/taskprioritymaster/list")]
        public async Task<IActionResult> GetAllTaskPriorityMaster()
        {
            var taskPriorityMaster = await context.TaskPriorities.ToListAsync();
            return Ok(taskPriorityMaster);
        }
        #endregion
    }
}
