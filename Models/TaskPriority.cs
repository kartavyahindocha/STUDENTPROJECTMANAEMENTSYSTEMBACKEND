using STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Models
{
    // NOTE: named "TaskPriorityMaster" for naming consistency with
    // TaskStatusMaster (TaskPriority itself has no BCL collision, but keeping
    // both "Master" tables named the same way avoids confusion).
    [Table("SPM_TaskPriority")]
    public class TaskPriorityMaster : BaseModel
    {
        [Key]
        public int TaskPriorityID { get; set; }

        // e.g. "Critical", "Moderate", "Low"
        [Required]
        [StringLength(20)]
        public string TaskPriorityName { get; set; }

        // NOTE: the source PDF documents this column's name as
        // "TaskPriortyCssClass" (missing an 'i') under SPM_TaskPriority,
        // while the equivalent SPM_TaskStatus column is spelled correctly
        // ("TaskStatusCssClass"). Kept exactly as documented so this maps to
        // the real database column name — rename both the property and the
        // source table if you correct the typo at the DB level.
        [Required]
        [StringLength(20)]
        public string TaskPriortyCssClass { get; set; }

        // ---- Relationships ----

        // One TaskPriorityMaster -> Many Task (1:N).
        public ICollection<Tasks> Tasks { get; set; } = new List<Tasks>();
    }
}
