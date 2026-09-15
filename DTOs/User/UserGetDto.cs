namespace STUDENTPROJECTMANAEMENTSYSTEMBACKEND.DTOs.User
{
    /// <summary>
    /// Used for GET responses — includes all fields plus FK display names.
    /// Password is excluded for security on GET.
    /// </summary>
    public class UserGetDto
    {
        public int UserID { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string? UserCode { get; set; }
        public string Email { get; set; } = string.Empty;
        public string MobileNumber { get; set; } = string.Empty;
        public string ProfilePicturePath { get; set; } = "/images/default-user.png";
        public bool IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public int UserTypeID { get; set; }
        public string? UserTypeName { get; set; }
    }
}
