using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Data;

namespace STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext context;
        #region DI
        public UserController(AppDbContext context)
        {
            this.context = context;
        }
        #endregion

        #region GetAllUser
        [HttpGet("/user/list")]
        public async Task<IActionResult>GetAllUser()
        {
            var students = await context.Users.ToListAsync();
            return Ok(students);
        }
        #endregion
    }
}
