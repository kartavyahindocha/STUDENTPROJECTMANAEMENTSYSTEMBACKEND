using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Data;
using STUDENTPROJECTMANAEMENTSYSTEMBACKEND.DTOs.User;
using STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Models;

namespace STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext context;
        private readonly IValidator<UserCreateEditDto> validator;

        #region DI
        public UserController(AppDbContext context, IValidator<UserCreateEditDto> validator)
        {
            this.context = context;
            this.validator = validator;
        }
        #endregion

        // Helper: map User entity → UserGetDto (no password, includes UserTypeName)
        private static UserGetDto ToDto(User u) => new UserGetDto
        {
            UserID             = u.UserID,
            FullName           = u.FullName,
            UserCode           = u.UserCode,
            Email              = u.Email,
            MobileNumber       = u.MobileNumber,
            ProfilePicturePath = u.ProfilePicturePath,
            IsActive           = u.IsActive,
            IsDeleted          = u.IsDeleted,
            UserTypeID         = u.UserTypeID,
            UserTypeName       = u.UserType?.UserTypeName
        };

        #region GetAllUser
        [HttpGet("/user/list")]
        public async Task<IActionResult> GetAllUser()
        {
            var users = await context.Users
                .Include(u => u.UserType)
                .AsNoTracking()
                .ToListAsync();

            var dtoList = users.Select(ToDto).ToList();

            var response = ApiResponse.FromResponse(dtoList, "User list fetched successfully", "No users found");
            return StatusCode(response.StatusCode, response);
        }
        #endregion

        #region GetFacultyList
        [HttpGet("/user/facultylist")]
        [HttpGet("/faculty/list")]
        public async Task<IActionResult> GetFacultyList()
        {
            var allUsers = await context.Users
                .Include(u => u.UserType)
                .Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
                .AsNoTracking()
                .ToListAsync();

            var facultyList = allUsers
                .Where(u => (u.UserType != null && (u.UserType.UserTypeName.Contains("Faculty", StringComparison.OrdinalIgnoreCase)
                                                 || u.UserType.UserTypeName.Contains("Teacher", StringComparison.OrdinalIgnoreCase)
                                                 || u.UserType.UserTypeName.Contains("Professor", StringComparison.OrdinalIgnoreCase)))
                         || u.UserRoles.Any(ur => ur.Role != null && ur.Role.RoleName.Contains("Faculty", StringComparison.OrdinalIgnoreCase)))
                .ToList();

            if (!facultyList.Any()) facultyList = allUsers;

            var dtoList = facultyList.Select(ToDto).ToList();
            var response = ApiResponse.FromResponse(dtoList, "Faculty list fetched successfully", "No faculty found");
            return StatusCode(response.StatusCode, response);
        }
        #endregion

        #region GetStudentList
        [HttpGet("/user/studentlist")]
        [HttpGet("/student/list")]
        public async Task<IActionResult> GetStudentList()
        {
            var allUsers = await context.Users
                .Include(u => u.UserType)
                .Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
                .AsNoTracking()
                .ToListAsync();

            var studentList = allUsers
                .Where(u => (u.UserType != null && u.UserType.UserTypeName.Contains("Student", StringComparison.OrdinalIgnoreCase))
                         || u.UserRoles.Any(ur => ur.Role != null && ur.Role.RoleName.Contains("Student", StringComparison.OrdinalIgnoreCase)))
                .ToList();

            if (!studentList.Any()) studentList = allUsers;

            var dtoList = studentList.Select(ToDto).ToList();
            var response = ApiResponse.FromResponse(dtoList, "Student list fetched successfully", "No students found");
            return StatusCode(response.StatusCode, response);
        }
        #endregion

        #region CreateUser
        [HttpPost("/user/create")]
        public async Task<IActionResult> CreateUser([FromBody] UserCreateEditDto user)
        {
            if (user == null)
            {
                var badRequestResponse = ApiResponse.BadRequest("INVALID DATA");
                return StatusCode(badRequestResponse.StatusCode, badRequestResponse);
            }

            var validationResult = await validator.ValidateAsync(user);
            if (!validationResult.IsValid)
            {
                var badRequestResponse = ApiResponse.BadRequest("Validation Failed", validationResult.Errors.Select(e => e.ErrorMessage));
                return StatusCode(badRequestResponse.StatusCode, badRequestResponse);
            }

            var userToAdd = new User
            {
                FullName           = user.FullName,
                UserCode           = user.UserCode,
                Email              = user.Email,
                Password           = user.Password,
                MobileNumber       = user.MobileNumber,
                ProfilePicturePath = string.IsNullOrEmpty(user.ProfilePicturePath) ? "/images/default-user.png" : user.ProfilePicturePath,
                IsActive           = user.IsActive,
                IsDeleted          = user.IsDeleted,
                UserTypeID         = user.UserTypeID
            };

            await context.Users.AddAsync(userToAdd);
            await context.SaveChangesAsync();

            var response = ApiResponse.Created(userToAdd, "User created successfully");
            return StatusCode(response.StatusCode, response);
        }
        #endregion

        #region GetUserById
        [HttpGet("/user/getbyid/{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await context.Users
                .Include(u => u.UserType)
                .FirstOrDefaultAsync(u => u.UserID == id);

            if (user == null)
            {
                var notFound = ApiResponse.NotFound("User Not Found");
                return StatusCode(notFound.StatusCode, notFound);
            }

            var dto = ToDto(user);
            var response = ApiResponse.Success(dto, "User fetched successfully");
            return StatusCode(response.StatusCode, response);
        }
        #endregion

        #region UpdateUser
        [HttpPut("/user/update/{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserCreateEditDto user)
        {
            if (user == null)
            {
                var badRequestResponse = ApiResponse.BadRequest("INVALID DATA");
                return StatusCode(badRequestResponse.StatusCode, badRequestResponse);
            }

            var validationResult = await validator.ValidateAsync(user);
            if (!validationResult.IsValid)
            {
                var badRequestResponse = ApiResponse.BadRequest("Validation Failed", validationResult.Errors.Select(e => e.ErrorMessage));
                return StatusCode(badRequestResponse.StatusCode, badRequestResponse);
            }

            var existingUser = await context.Users.FindAsync(id);
            if (existingUser == null)
            {
                var notFoundResponse = ApiResponse.NotFound("User Not Found");
                return StatusCode(notFoundResponse.StatusCode, notFoundResponse);
            }

            existingUser.FullName           = user.FullName;
            existingUser.UserCode           = user.UserCode;
            existingUser.Email              = user.Email;
            existingUser.Password           = user.Password;
            existingUser.MobileNumber       = user.MobileNumber;
            existingUser.ProfilePicturePath = string.IsNullOrEmpty(user.ProfilePicturePath) ? existingUser.ProfilePicturePath : user.ProfilePicturePath;
            existingUser.IsActive           = user.IsActive;
            existingUser.IsDeleted          = user.IsDeleted;
            existingUser.UserTypeID         = user.UserTypeID;

            await context.SaveChangesAsync();

            var response = ApiResponse.Success(existingUser, "User updated successfully");
            return StatusCode(response.StatusCode, response);
        }
        #endregion

        #region DeleteUserByPK
        [HttpDelete("/user/delete/{id}")]
        public async Task<IActionResult> DeleteUserByPK(int id)
        {
            var user = await context.Users.FindAsync(id);
            if (user == null)
            {
                var notFoundResponse = ApiResponse.NotFound("User Not Found");
                return StatusCode(notFoundResponse.StatusCode, notFoundResponse);
            }

            context.Users.Remove(user);
            await context.SaveChangesAsync();

            var response = ApiResponse.Success(user, "User deleted successfully");
            return StatusCode(response.StatusCode, response);
        }
        #endregion
    }
}
