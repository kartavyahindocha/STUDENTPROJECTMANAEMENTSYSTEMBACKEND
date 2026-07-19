using STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Models
{
    // NOTE: named "Tasks" instead of "Task" — the plain name collides with
    // System.Threading.Tasks.Task, a real BCL type in scope via
    // ImplicitUsings. Table name still maps to SPM_Task.
    [Table("SPM_Task")]
    public class Tasks : BaseModel
    {
        [Key]
        public int TaskID { get; set; }

        [Required]
        [StringLength(200)]
        public string TaskTitle { get; set; }

        public string? TaskDescription { get; set; }

        [Required]
        [Column(TypeName = "decimal(5,2)")]
        public decimal AssignedScore { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal? EarnedScore { get; set; }

        [Required]
        [Column(TypeName = "decimal(5,2)")]
        public decimal ProgressPercentage { get; set; }

        [Required]
        public DateTime TaskAssignedDate { get; set; }

        public DateTime? TaskStartDate { get; set; }

        public DateTime? TaskDueDate { get; set; }

        public DateTime? TaskCompletedDate { get; set; }

        public DateTime? NextFollowUpDate { get; set; }

        [StringLength(500)]
        public string? FacultyRemarks { get; set; }

        [StringLength(500)]
        public string? StudentRemarks { get; set; }

        // ---- Foreign Keys ----
        // [ForeignKey("<IdColumnName>")] is placed on the navigation property
        // and points at the actual FK id column declared just above it.

        [Required]
        public int ProjectAllocationID { get; set; }

        [ForeignKey("ProjectAllocationID")]
        public ProjectAllocation? ProjectAllocation { get; set; }

        [Required]
        public int TaskStatusID { get; set; }

        [ForeignKey("TaskStatusID")]
        public TaskStatusMaster? TaskStatus { get; set; }

        [Required]
        public int TaskPriorityID { get; set; }

        [ForeignKey("TaskPriorityID")]
        public TaskPriorityMaster? TaskPriority { get; set; }
    }
}
