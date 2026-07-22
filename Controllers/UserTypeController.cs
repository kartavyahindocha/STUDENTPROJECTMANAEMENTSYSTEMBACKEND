using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Data;
using STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Models;

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
            var userType = await context.UserTypes.AsNoTracking().ToListAsync();
            return Ok(userType);
        }
        #endregion

        #region CreateUserType
        [HttpPost("/usertype/create")]
        public async Task<IActionResult> CreateUserType([FromBody]UserTypeDto userType)
        {
            if(userType==null)
            {
                return BadRequest("UserType data is empty");
            }
            var user = new UserType();
            user.UserTypeName = userType.UserTypeName;
            user.Description = userType.Description;

            var createuser = context.Add(user);
            await context.SaveChangesAsync();

            return Ok(createuser);
        }
        #endregion

        #region GetById

        [HttpGet("/usertype/getbyid/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            if(id==null)
            {
                return BadRequest("Id Not Found"); 
            }
            var usertype=await context.UserTypes.FindAsync(id);
            return Ok(usertype);
        }
        #endregion

        #region DeleteUserType
        [HttpDelete("/usertype/delete/{id}")]
        public async Task<IActionResult> DeleteUserType(int id)
        {
            if(id== null)
            {
                return BadRequest("Id Not Found");
            }
            var userTypeId = await context.UserTypes.FindAsync(id);
            if(userTypeId == null)
            {
                return NotFound("UserType Not Found");
            }
            var deleteUserType = context.UserTypes.Remove(userTypeId);
            return Ok(deleteUserType);
        }
        #endregion
    }
}
