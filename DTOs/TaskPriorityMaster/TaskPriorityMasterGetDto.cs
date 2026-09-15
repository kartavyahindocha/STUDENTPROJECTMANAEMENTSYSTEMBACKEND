namespace STUDENTPROJECTMANAEMENTSYSTEMBACKEND.DTOs.TaskPriorityMaster
{
    /// <summary>
    /// Used for GET responses.
    /// </summary>
    public class TaskPriorityMasterGetDto
    {
        public int TaskPriorityID { get; set; }
        public string TaskPriorityName { get; set; } = string.Empty;
        public string TaskPriortyCssClass { get; set; } = string.Empty;
    }
}
