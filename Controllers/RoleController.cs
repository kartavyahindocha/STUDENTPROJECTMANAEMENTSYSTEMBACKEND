using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Data;
using STUDENTPROJECTMANAEMENTSYSTEMBACKEND.DTOs.Role;
using STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Models;

namespace STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly AppDbContext context;
        private readonly IValidator<RoleCreateEditDto> validator;

        #region DI
        public RoleController(AppDbContext context, IValidator<RoleCreateEditDto> validator)
        {
            this.context = context;
            this.validator = validator;
        }
        #endregion

        #region GetAllRole
        [HttpGet("/role/list")]
        public async Task<IActionResult> GetAllRole()
        {
            var roles = await context.Roles.AsNoTracking().ToListAsync();

            var dtoList = roles.Select(r => new RoleGetDto
            {
                RoleID      = r.RoleID,
                RoleName    = r.RoleName,
                Description = r.Description
            }).ToList();

            var response = ApiResponse.FromResponse(dtoList, "Role list fetched successfully", "No roles found");
            return StatusCode(response.StatusCode, response);
        }
        #endregion

        #region CreateRole
        [HttpPost("/role/create")]
        public async Task<IActionResult> CreateRole([FromBody] RoleCreateEditDto roles)
        {
            if (roles == null)
            {
                var badRequestResponse = ApiResponse.BadRequest("INVALID DATA");
                return StatusCode(badRequestResponse.StatusCode, badRequestResponse);
            }

            var validationResult = await validator.ValidateAsync(roles);
            if (!validationResult.IsValid)
            {
                var badRequestResponse = ApiResponse.BadRequest("Validation Failed", validationResult.Errors.Select(e => e.ErrorMessage));
                return StatusCode(badRequestResponse.StatusCode, badRequestResponse);
            }

            var rolestoAdd = new Role
            {
                RoleName    = roles.RoleName,
                Description = roles.Description
            };

            await context.Roles.AddAsync(rolestoAdd);
            await context.SaveChangesAsync();

            var response = ApiResponse.Created(rolestoAdd, "Role created successfully");
            return StatusCode(response.StatusCode, response);
        }
        #endregion

        #region GetRoleById
        [HttpGet("/role/getbyid/{id}")]
        public async Task<IActionResult> GetRoleById(int id)
        {
            var role = await context.Roles.FindAsync(id);
            if (role == null)
            {
                var notFound = ApiResponse.NotFound("Role Not Found");
                return StatusCode(notFound.StatusCode, notFound);
            }

            var dto = new RoleGetDto
            {
                RoleID      = role.RoleID,
                RoleName    = role.RoleName,
                Description = role.Description
            };

            var response = ApiResponse.Success(dto, "Role fetched successfully");
            return StatusCode(response.StatusCode, response);
        }
        #endregion

        #region UpdateRole
        [HttpPut("/role/update/{id}")]
        public async Task<IActionResult> UpdateRole(int id, [FromBody] RoleCreateEditDto roles)
        {
            if (roles == null)
            {
                var badRequestResponse = ApiResponse.BadRequest("INVALID DATA");
                return StatusCode(badRequestResponse.StatusCode, badRequestResponse);
            }

            var validationResult = await validator.ValidateAsync(roles);
            if (!validationResult.IsValid)
            {
                var badRequestResponse = ApiResponse.BadRequest("Validation Failed", validationResult.Errors.Select(e => e.ErrorMessage));
                return StatusCode(badRequestResponse.StatusCode, badRequestResponse);
            }

            var existingRole = await context.Roles.FindAsync(id);
            if (existingRole == null)
            {
                var notFoundResponse = ApiResponse.NotFound("Role Not Found");
                return StatusCode(notFoundResponse.StatusCode, notFoundResponse);
            }

            existingRole.RoleName    = roles.RoleName;
            existingRole.Description = roles.Description;

            await context.SaveChangesAsync();

            var response = ApiResponse.Success(existingRole, "Role updated successfully");
            return StatusCode(response.StatusCode, response);
        }
        #endregion

        #region DeleteRoleByPK
        [HttpDelete("/role/delete/{id}")]
        public async Task<IActionResult> DeleteRoleByPK(int id)
        {
            var role = await context.Roles.FindAsync(id);
            if (role == null)
            {
                var notFoundResponse = ApiResponse.NotFound("Role Not Found");
                return StatusCode(notFoundResponse.StatusCode, notFoundResponse);
            }

            context.Roles.Remove(role);
            await context.SaveChangesAsync();

            var response = ApiResponse.Success(role, "Role deleted successfully");
            return StatusCode(response.StatusCode, response);
        }
        #endregion
    }
}
