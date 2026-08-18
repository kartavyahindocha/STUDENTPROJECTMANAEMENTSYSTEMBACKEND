namespace STUDENTPROJECTMANAEMENTSYSTEMBACKEND.DTOs.User
{
    /// <summary>
    /// Used for POST (Create) and PUT (Update) requests — includes password field for input.
    /// </summary>
    public class UserCreateEditDto
    {
        public string FullName { get; set; } = string.Empty;
        public string? UserCode { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string MobileNumber { get; set; } = string.Empty;
        public string ProfilePicturePath { get; set; } = "/images/default-user.png";
        public bool IsActive { get; set; } = true;
        public bool? IsDeleted { get; set; } = false;
        public int UserTypeID { get; set; }
    }
}
