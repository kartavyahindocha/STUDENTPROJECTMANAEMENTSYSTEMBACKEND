using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Data;
using STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Models;

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
            var role = await context.Roles.AsNoTracking().ToListAsync();
            return Ok(role);
        }
        #endregion

        #region CreateRole
        [HttpPost("/role/create")]
        public async Task<IActionResult> CreateRole([FromBody] RoleDTO roles)
        {
            if(roles==null)
            {
                return BadRequest("INVALID DATA");
            }
            var rolestoAdd = new Role();
            rolestoAdd.RoleName = roles.RoleName;
            rolestoAdd.Description = roles.Description;

            var user = context.Roles.AddAsync(rolestoAdd);
            await context.SaveChangesAsync();

            return Ok(rolestoAdd);
        }
        #endregion

        #region GetById
        [HttpGet("/role/getbyid/{id}")]
        public async Task<IActionResult> GetRoleById(int id)
        {
            var roleid = await context.Roles.FindAsync(id);
            if (roleid == null)
            {
                return NotFound("Role Not Found");
            }
            return Ok(roleid);
        }
        #endregion

        #region DeleteRole
        [HttpDelete("/role/delete/{id}")]
        public async Task<IActionResult>DeleteRoleByPK(int id)
        {
            var roleid=await context.Roles.FindAsync(id);

            if(roleid== null)
            {
                return NotFound("Role Not Found");
            }

            var roleDelete=context.Roles.Remove(roleid);
            await context.SaveChangesAsync();
            return Ok(roleDelete);
        }
        #endregion
    }
}
