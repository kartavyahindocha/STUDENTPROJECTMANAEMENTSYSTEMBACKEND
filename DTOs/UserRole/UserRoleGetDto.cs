namespace STUDENTPROJECTMANAEMENTSYSTEMBACKEND.DTOs.UserRole
{
    /// <summary>
    /// Used for GET responses — includes FK display names.
    /// </summary>
    public class UserRoleGetDto
    {
        public int RolePermissionID { get; set; }
        public int RoleID { get; set; }
        public string? RoleName { get; set; }
        public int UserID { get; set; }
        public string? UserFullName { get; set; }
    }
}
