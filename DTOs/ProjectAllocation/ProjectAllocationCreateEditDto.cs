namespace STUDENTPROJECTMANAEMENTSYSTEMBACKEND.DTOs.ProjectAllocation
{
    /// <summary>
    /// Used for POST (Create) and PUT (Update) requests — contains only input fields.
    /// </summary>
    public class ProjectAllocationCreateEditDto
    {
        public DateTime AssignedDate { get; set; }
        public DateTime ProjectStartDate { get; set; }
        public DateTime ProjectEndDate { get; set; }
        public int TotalTasksGiven { get; set; }
        public int TotalCompletedTasks { get; set; }
        public decimal ProgressPercentage { get; set; }
        public string? OverAllGrade { get; set; }
        public int ProjectID { get; set; }
        public int StudentID { get; set; }
        public int FacultyID { get; set; }
    }
}
