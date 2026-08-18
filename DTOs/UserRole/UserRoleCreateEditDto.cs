namespace STUDENTPROJECTMANAEMENTSYSTEMBACKEND.DTOs.UserRole
{
    /// <summary>
    /// Used for POST (Create) and PUT (Update) requests.
    /// </summary>
    public class UserRoleCreateEditDto
    {
        public int RoleID { get; set; }
        public int UserID { get; set; }
    }
}
