namespace STUDENTPROJECTMANAEMENTSYSTEMBACKEND.DTOs.ProjectMaster
{
    /// <summary>
    /// Used for POST (Create) and PUT (Update) requests — includes title, description, faculty, and student IDs.
    /// </summary>
    public class ProjectMasterCreateEditDto
    {
        public string ProjectTitle { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int? FacultyID { get; set; }
        public int? StudentID { get; set; }
    }
}
