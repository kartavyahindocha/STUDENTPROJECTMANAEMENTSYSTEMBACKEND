using STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Models
{
    [Table("SPM_Project")]
    public class Project : BaseModel
    {
        [Key]
        public int ProjectId { get; set; }

        [Required]
        [StringLength(200)]
        public string ProjectTitle { get; set; }

        public string? Description { get; set; }

        [Required]
        public DateTime AssignedDate { get; set; }

        public bool? IsDeleted { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public int TotalTasks { get; set; }

        [Required]
        public int CompletedTasks { get; set; }

        [Required]
        [Column(TypeName = "decimal(5,2)")]
        public decimal ProgressPercentage { get; set; }

        // ---- Foreign Keys ----
        // [ForeignKey("<IdColumnName>")] is placed on the navigation property
        // and points at the actual FK id column declared just above it.

        [Required]
        public int StudentId { get; set; }

        [ForeignKey("StudentId")]
        public User? Student { get; set; }

        [Required]
        public int FacultyId { get; set; }

        [ForeignKey("FacultyId")]
        public User? Faculty { get; set; }

        [Required]
        public int ProjectStatus { get; set; }

        [ForeignKey("ProjectStatus")]
        public Status? Status { get; set; }
    }
}
