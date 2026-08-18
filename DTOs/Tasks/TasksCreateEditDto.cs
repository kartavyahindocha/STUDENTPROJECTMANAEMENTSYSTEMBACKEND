namespace STUDENTPROJECTMANAEMENTSYSTEMBACKEND.DTOs.Tasks
{
    /// <summary>
    /// Used for POST (Create) and PUT (Update) requests — contains only input FK IDs.
    /// </summary>
    public class TasksCreateEditDto
    {
        public string TaskTitle { get; set; } = string.Empty;
        public string? TaskDescription { get; set; }
        public decimal AssignedScore { get; set; }
        public decimal? EarnedScore { get; set; }
        public decimal ProgressPercentage { get; set; }
        public DateTime TaskAssignedDate { get; set; }
        public DateTime? TaskStartDate { get; set; }
        public DateTime? TaskDueDate { get; set; }
        public DateTime? TaskCompletedDate { get; set; }
        public DateTime? NextFollowUpDate { get; set; }
        public string? FacultyRemarks { get; set; }
        public string? StudentRemarks { get; set; }
        public int ProjectAllocationID { get; set; }
        public int TaskStatusID { get; set; }
        public int TaskPriorityID { get; set; }
    }
}
