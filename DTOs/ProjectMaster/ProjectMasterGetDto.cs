namespace STUDENTPROJECTMANAEMENTSYSTEMBACKEND.DTOs.ProjectMaster
{
    /// <summary>
    /// Used for GET responses — includes project ID, display fields, and allocation info.
    /// </summary>
    public class ProjectMasterGetDto
    {
        public int ProjectID { get; set; }
        public string ProjectTitle { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int? FacultyID { get; set; }
        public string? FacultyName { get; set; }
        public int? StudentID { get; set; }
        public string? StudentName { get; set; }
    }
}
