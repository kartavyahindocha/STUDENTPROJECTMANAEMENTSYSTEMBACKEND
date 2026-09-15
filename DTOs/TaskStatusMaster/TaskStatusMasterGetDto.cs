namespace STUDENTPROJECTMANAEMENTSYSTEMBACKEND.DTOs.TaskStatusMaster
{
    /// <summary>
    /// Used for GET responses.
    /// </summary>
    public class TaskStatusMasterGetDto
    {
        public int TaskStatusID { get; set; }
        public string TaskStatusName { get; set; } = string.Empty;
        public string TaskStatusCssClass { get; set; } = string.Empty;
    }
}
