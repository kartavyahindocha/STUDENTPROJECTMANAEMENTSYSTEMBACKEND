namespace STUDENTPROJECTMANAEMENTSYSTEMBACKEND.DTOs.UserType
{
    /// <summary>
    /// Used for GET responses.
    /// </summary>
    public class UserTypeGetDto
    {
        public int UserTypeID { get; set; }
        public string UserTypeName { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
