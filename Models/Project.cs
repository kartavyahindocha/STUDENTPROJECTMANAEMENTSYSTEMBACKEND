using STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace STUDENTPROJECTMANAGEMENTSYSTEM.Models
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

        [Required]
        [ForeignKey(nameof(Student))]
        public int StudentId { get; set; }
        public User? Student { get; set; }

        [Required]
        [ForeignKey(nameof(Faculty))]
        public int FacultyId { get; set; }
        public User? Faculty { get; set; }

        [Required]
        [ForeignKey(nameof(Status))]
        public int ProjectStatus { get; set; }
        public Status? Status { get; set; }
    }
}
