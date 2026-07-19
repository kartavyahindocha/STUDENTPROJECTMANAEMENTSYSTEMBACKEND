using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Data;

namespace STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly AppDbContext context;
        #region DI
        public TasksController(AppDbContext context)
        {
            this.context = context;
        }
        #endregion

        #region GetAllTasks
        [HttpGet("/tasks/list")]
        public async Task<IActionResult> GetAllTasks()
        {
            var tasks = await context.Tasks.ToListAsync();
            return Ok(tasks);
        }
        #endregion
    }
}
