namespace STUDENTPROJECTMANAEMENTSYSTEMBACKEND.DTOs.Role
{
    /// <summary>
    /// Used for GET responses.
    /// </summary>
    public class RoleGetDto
    {
        public int RoleID { get; set; }
        public string RoleName { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
