namespace STUDENTPROJECTMANAEMENTSYSTEMBACKEND.DTOs.TaskPriorityMaster
{
    /// <summary>
    /// Used for POST (Create) and PUT (Update) requests.
    /// </summary>
    public class TaskPriorityMasterCreateEditDto
    {
        public string TaskPriorityName { get; set; } = string.Empty;
        public string TaskPriortyCssClass { get; set; } = string.Empty;
    }
}
