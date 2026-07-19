using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Data;

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
            var taskStatusMaster = await context.TaskStatus.ToListAsync();
            return Ok(taskStatusMaster);
        }
        #endregion
    }
}
