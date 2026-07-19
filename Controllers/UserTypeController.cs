using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Data;

namespace STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserTypeController : ControllerBase
    {
        private readonly AppDbContext context;
        #region DI
        public UserTypeController(AppDbContext context)
        {
            this.context = context;
        }
        #endregion

        #region GetAllUserType
        [HttpGet("/usertype/list")]
        public async Task<IActionResult> GetAllUserType()
        {
            var userType = await context.UserTypes.ToListAsync();
            return Ok(userType);
        }
        #endregion
    }
}
