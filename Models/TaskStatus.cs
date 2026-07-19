using STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Models
{
    // NOTE: named "TaskStatusMaster" instead of "TaskStatus" — the plain name
    // collides with System.Threading.Tasks.TaskStatus, a real BCL enum that's
    // in scope via ImplicitUsings. Table name still maps to SPM_TaskStatus.
    [Table("SPM_TaskStatus")]
    public class TaskStatusMaster : BaseModel
    {
        [Key]
        public int TaskStatusID { get; set; }

        // e.g. "Ongoing", "Cancelled", "Completed", "Pending"
        [Required]
        [StringLength(20)]
        public string TaskStatusName { get; set; }

        // Bootstrap/utility CSS class used to render this status as a badge
        // consistently wherever it's shown (e.g. "tone-success").
        [Required]
        [StringLength(100)]
        public string TaskStatusCssClass { get; set; }

        // ---- Relationships ----

        // One TaskStatusMaster -> Many Task (1:N).
        public ICollection<Tasks> Tasks { get; set; } = new List<Tasks>();
    }
}
