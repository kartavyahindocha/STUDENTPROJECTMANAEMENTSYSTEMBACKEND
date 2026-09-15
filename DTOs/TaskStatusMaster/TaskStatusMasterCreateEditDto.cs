namespace STUDENTPROJECTMANAEMENTSYSTEMBACKEND.DTOs.TaskStatusMaster
{
    /// <summary>
    /// Used for POST (Create) and PUT (Update) requests.
    /// </summary>
    public class TaskStatusMasterCreateEditDto
    {
        public string TaskStatusName { get; set; } = string.Empty;
        public string TaskStatusCssClass { get; set; } = string.Empty;
    }
}
