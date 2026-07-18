using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Data;

namespace STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        private readonly AppDbContext context;
        #region DI
        public StatusController(AppDbContext context)
        {
            this.context = context;
        }
        #endregion

        #region GETALLSTATUS
        [HttpGet("/status/list")]
        public async Task<IActionResult> GetAllStatus()
        {
            var status=await context.Status.ToListAsync();
            return Ok(status);
        }
        #endregion
    }
}
