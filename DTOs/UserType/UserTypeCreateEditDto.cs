namespace STUDENTPROJECTMANAEMENTSYSTEMBACKEND.DTOs.UserType
{
    /// <summary>
    /// Used for POST (Create) and PUT (Update) requests.
    /// </summary>
    public class UserTypeCreateEditDto
    {
        public string UserTypeName { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
