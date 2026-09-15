namespace STUDENTPROJECTMANAEMENTSYSTEMBACKEND.DTOs.Role
{
    /// <summary>
    /// Used for POST (Create) and PUT (Update) requests.
    /// </summary>
    public class RoleCreateEditDto
    {
        public string RoleName { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
