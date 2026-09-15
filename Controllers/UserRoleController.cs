using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Data;
using STUDENTPROJECTMANAEMENTSYSTEMBACKEND.DTOs.UserRole;
using STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Models;

namespace STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserRoleController : ControllerBase
    {
        private readonly AppDbContext context;
        private readonly IValidator<UserRoleCreateEditDto> validator;

        #region DI
        public UserRoleController(AppDbContext context, IValidator<UserRoleCreateEditDto> validator)
        {
            this.context = context;
            this.validator = validator;
        }
        #endregion

        #region GetAllUserRole
        [HttpGet("/userrole/list")]
        public async Task<IActionResult> GetAllUserRole()
        {
            var userRoles = await context.UserRoles
                .Include(ur => ur.Role)
                .Include(ur => ur.User)
                .AsNoTracking()
                .ToListAsync();

            var dtoList = userRoles.Select(ur => new UserRoleGetDto
            {
                RolePermissionID = ur.RolePermissionID,
                RoleID           = ur.RoleID,
                RoleName         = ur.Role?.RoleName,
                UserID           = ur.UserID,
                UserFullName     = ur.User?.FullName
            }).ToList();

            var response = ApiResponse.FromResponse(dtoList, "UserRole list fetched successfully", "No UserRoles found");
            return StatusCode(response.StatusCode, response);
        }
        #endregion

        #region CreateUserRole
        [HttpPost("/userrole/create")]
        public async Task<IActionResult> CreateUserRole([FromBody] UserRoleCreateEditDto userRole)
        {
            if (userRole == null)
            {
                var badRequestResponse = ApiResponse.BadRequest("INVALID DATA");
                return StatusCode(badRequestResponse.StatusCode, badRequestResponse);
            }

            var validationResult = await validator.ValidateAsync(userRole);
            if (!validationResult.IsValid)
            {
                var badRequestResponse = ApiResponse.BadRequest("Validation Failed", validationResult.Errors.Select(e => e.ErrorMessage));
                return StatusCode(badRequestResponse.StatusCode, badRequestResponse);
            }

            var userRoleToAdd = new UserRole
            {
                RoleID = userRole.RoleID,
                UserID = userRole.UserID
            };

            await context.UserRoles.AddAsync(userRoleToAdd);
            await context.SaveChangesAsync();

            var response = ApiResponse.Created(userRoleToAdd, "UserRole created successfully");
            return StatusCode(response.StatusCode, response);
        }
        #endregion

        #region GetUserRoleById
        [HttpGet("/userrole/getbyid/{id}")]
        public async Task<IActionResult> GetUserRoleById(int id)
        {
            var ur = await context.UserRoles
                .Include(ur => ur.Role)
                .Include(ur => ur.User)
                .FirstOrDefaultAsync(ur => ur.RolePermissionID == id);

            if (ur == null)
            {
                var notFound = ApiResponse.NotFound("UserRole Not Found");
                return StatusCode(notFound.StatusCode, notFound);
            }

            var dto = new UserRoleGetDto
            {
                RolePermissionID = ur.RolePermissionID,
                RoleID           = ur.RoleID,
                RoleName         = ur.Role?.RoleName,
                UserID           = ur.UserID,
                UserFullName     = ur.User?.FullName
            };

            var response = ApiResponse.Success(dto, "UserRole fetched successfully");
            return StatusCode(response.StatusCode, response);
        }
        #endregion

        #region UpdateUserRole
        [HttpPut("/userrole/update/{id}")]
        public async Task<IActionResult> UpdateUserRole(int id, [FromBody] UserRoleCreateEditDto userRole)
        {
            if (userRole == null)
            {
                var badRequestResponse = ApiResponse.BadRequest("INVALID DATA");
                return StatusCode(badRequestResponse.StatusCode, badRequestResponse);
            }

            var validationResult = await validator.ValidateAsync(userRole);
            if (!validationResult.IsValid)
            {
                var badRequestResponse = ApiResponse.BadRequest("Validation Failed", validationResult.Errors.Select(e => e.ErrorMessage));
                return StatusCode(badRequestResponse.StatusCode, badRequestResponse);
            }

            var existingUserRole = await context.UserRoles.FindAsync(id);
            if (existingUserRole == null)
            {
                var notFoundResponse = ApiResponse.NotFound("UserRole Not Found");
                return StatusCode(notFoundResponse.StatusCode, notFoundResponse);
            }

            existingUserRole.RoleID = userRole.RoleID;
            existingUserRole.UserID = userRole.UserID;

            await context.SaveChangesAsync();

            var response = ApiResponse.Success(existingUserRole, "UserRole updated successfully");
            return StatusCode(response.StatusCode, response);
        }
        #endregion

        #region DeleteUserRoleByPK
        [HttpDelete("/userrole/delete/{id}")]
        public async Task<IActionResult> DeleteUserRoleByPK(int id)
        {
            var userRole = await context.UserRoles.FindAsync(id);
            if (userRole == null)
            {
                var notFoundResponse = ApiResponse.NotFound("UserRole Not Found");
                return StatusCode(notFoundResponse.StatusCode, notFoundResponse);
            }

            context.UserRoles.Remove(userRole);
            await context.SaveChangesAsync();

            var response = ApiResponse.Success(userRole, "UserRole deleted successfully");
            return StatusCode(response.StatusCode, response);
        }
        #endregion
    }
}
