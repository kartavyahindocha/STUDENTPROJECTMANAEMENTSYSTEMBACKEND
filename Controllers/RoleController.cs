using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Data;

namespace STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly AppDbContext context;
        #region DI
        public RoleController(AppDbContext context)
        {
            this.context = context;
        }
        #endregion

        #region GetAllRole
        [HttpGet("/role/list")]
        public async Task<IActionResult> GetAllRole()
        {
            var role = await context.Roles.ToListAsync();
            return Ok(role);
        }
        #endregion
    }
}
