using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Data;

namespace STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRoleController : ControllerBase
    {
        private readonly AppDbContext context;
        #region DI
        public UserRoleController(AppDbContext context)
        {
            this.context = context;
        }
        #endregion

        #region GetAllUserRole
        [HttpGet("/userrole/list")]
        public async Task<IActionResult> GetAllUserRole()
        {
            var userRole = await context.UserRoles.ToListAsync();
            return Ok(userRole);
        }
        #endregion
    }
}
