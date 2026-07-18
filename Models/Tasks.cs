using STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace STUDENTPROJECTMANAGEMENTSYSTEM.Models
{
    [Table("SPM_Task")]
    public class Tasks : BaseModel
    {
        [Key]
        public int TaskId { get; set; }

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

        public DateTime? StartDate { get; set; }

        public DateTime? DueDate { get; set; }

        public DateTime? CompletedDate { get; set; }

        [StringLength(500)]
        public string? FacultyRemarks { get; set; }

        [StringLength(500)]
        public string? StudentRemarks { get; set; }

        public bool? IsDeleted { get; set; }

        // ---- Foreign Keys ----

        // NOTE: the PDF documents this column as "FK -> Allocation", but no
        // SPM_Allocation table appears anywhere in the provided schema pages —
        // only SPM_Role, SPM_User, SPM_UserRole, SPM_Status, SPM_Priority,
        // SPM_Project, and SPM_Task were given. Left as a plain required int
        // with no navigation property since the target table isn't defined.
        [Required]
        public int AllocationID { get; set; }

        [Required]
        [ForeignKey(nameof(Status))]
        public int TaskStatus { get; set; }
        public Status? Status { get; set; }

        [Required]
        [ForeignKey(nameof(Priority))]
        public int PriorityID { get; set; }
        public Priority? Priority { get; set; }
    }
}
