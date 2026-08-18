using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Data;
using STUDENTPROJECTMANAEMENTSYSTEMBACKEND.DTOs.UserType;
using STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Models;

namespace STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserTypeController : ControllerBase
    {
        private readonly AppDbContext context;
        private readonly IValidator<UserTypeCreateEditDto> validator;

        #region DI
        public UserTypeController(AppDbContext context, IValidator<UserTypeCreateEditDto> validator)
        {
            this.context = context;
            this.validator = validator;
        }
        #endregion

        #region GetAllUserType
        [HttpGet("/usertype/list")]
        public async Task<IActionResult> GetAllUserType()
        {
            var userTypes = await context.UserTypes.AsNoTracking().ToListAsync();

            var dtoList = userTypes.Select(ut => new UserTypeGetDto
            {
                UserTypeID   = ut.UserTypeID,
                UserTypeName = ut.UserTypeName,
                Description  = ut.Description
            }).ToList();

            var response = ApiResponse.FromResponse(dtoList, "UserType list fetched successfully", "No UserTypes found");
            return StatusCode(response.StatusCode, response);
        }
        #endregion

        #region CreateUserType
        [HttpPost("/usertype/create")]
        public async Task<IActionResult> CreateUserType([FromBody] UserTypeCreateEditDto userType)
        {
            if (userType == null)
            {
                var badRequestResponse = ApiResponse.BadRequest("UserType data is empty");
                return StatusCode(badRequestResponse.StatusCode, badRequestResponse);
            }

            var validationResult = await validator.ValidateAsync(userType);
            if (!validationResult.IsValid)
            {
                var badRequestResponse = ApiResponse.BadRequest("Validation Failed", validationResult.Errors.Select(e => e.ErrorMessage));
                return StatusCode(badRequestResponse.StatusCode, badRequestResponse);
            }

            var userTypeToAdd = new UserType
            {
                UserTypeName = userType.UserTypeName,
                Description  = userType.Description
            };

            await context.UserTypes.AddAsync(userTypeToAdd);
            await context.SaveChangesAsync();

            var response = ApiResponse.Created(userTypeToAdd, "UserType created successfully");
            return StatusCode(response.StatusCode, response);
        }
        #endregion

        #region GetUserTypeById
        [HttpGet("/usertype/getbyid/{id}")]
        public async Task<IActionResult> GetUserTypeById(int id)
        {
            var userType = await context.UserTypes.FindAsync(id);
            if (userType == null)
            {
                var notFound = ApiResponse.NotFound("UserType Not Found");
                return StatusCode(notFound.StatusCode, notFound);
            }

            var dto = new UserTypeGetDto
            {
                UserTypeID   = userType.UserTypeID,
                UserTypeName = userType.UserTypeName,
                Description  = userType.Description
            };

            var response = ApiResponse.Success(dto, "UserType fetched successfully");
            return StatusCode(response.StatusCode, response);
        }
        #endregion

        #region UpdateUserType
        [HttpPut("/usertype/update/{id}")]
        public async Task<IActionResult> UpdateUserType(int id, [FromBody] UserTypeCreateEditDto userType)
        {
            if (userType == null)
            {
                var badRequestResponse = ApiResponse.BadRequest("UserType data is empty");
                return StatusCode(badRequestResponse.StatusCode, badRequestResponse);
            }

            var validationResult = await validator.ValidateAsync(userType);
            if (!validationResult.IsValid)
            {
                var badRequestResponse = ApiResponse.BadRequest("Validation Failed", validationResult.Errors.Select(e => e.ErrorMessage));
                return StatusCode(badRequestResponse.StatusCode, badRequestResponse);
            }

            var existingUserType = await context.UserTypes.FindAsync(id);
            if (existingUserType == null)
            {
                var notFoundResponse = ApiResponse.NotFound("UserType Not Found");
                return StatusCode(notFoundResponse.StatusCode, notFoundResponse);
            }

            existingUserType.UserTypeName = userType.UserTypeName;
            existingUserType.Description  = userType.Description;

            await context.SaveChangesAsync();

            var response = ApiResponse.Success(existingUserType, "UserType updated successfully");
            return StatusCode(response.StatusCode, response);
        }
        #endregion

        #region DeleteUserTypeByPK
        [HttpDelete("/usertype/delete/{id}")]
        public async Task<IActionResult> DeleteUserTypeByPK(int id)
        {
            var userType = await context.UserTypes.FindAsync(id);
            if (userType == null)
            {
                var notFoundResponse = ApiResponse.NotFound("UserType Not Found");
                return StatusCode(notFoundResponse.StatusCode, notFoundResponse);
            }

            context.UserTypes.Remove(userType);
            await context.SaveChangesAsync();

            var response = ApiResponse.Success(userType, "UserType deleted successfully");
            return StatusCode(response.StatusCode, response);
        }
        #endregion
    }
}
