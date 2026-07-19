using STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Models
{
    [Table("SPM_ProjectAllocation")]
    public class ProjectAllocation : BaseModel
    {
        [Key]
        public int ProjectAllocationID { get; set; }

        // Not Null, Default GETDATE() — the DB-side default is configured in
        // AppDbContext via HasDefaultValueSql("GETDATE()"), since that has no
        // direct data-annotation equivalent.
        [Required]
        public DateTime AssignedDate { get; set; }

        [Required]
        public DateTime ProjectStartDate { get; set; }

        [Required]
        public DateTime ProjectEndDate { get; set; }

        [Required]
        public int TotalTasksGiven { get; set; }

        [Required]
        public int TotalCompletedTasks { get; set; }

        [Required]
        [Column(TypeName = "decimal(5,2)")]
        public decimal ProgressPercentage { get; set; }

        // Allow Null -> 'A', 'B', 'C' entered at the end of semester.
        [StringLength(1)]
        public string? OverAllGrade { get; set; }

        // ---- Foreign Keys ----
        // [ForeignKey("<IdColumnName>")] is placed on the navigation property
        // and points at the actual FK id column declared just above it.

        [Required]
        public int ProjectID { get; set; }

        [ForeignKey("ProjectID")]
        public ProjectMaster? ProjectMaster { get; set; }

        [Required]
        public int StudentID { get; set; }

        [ForeignKey("StudentID")]
        public User? Student { get; set; }

        [Required]
        public int FacultyID { get; set; }

        [ForeignKey("FacultyID")]
        public User? Faculty { get; set; }

        // ---- Relationships ----

        // One ProjectAllocation -> Many Task (1:N).
        public ICollection<Tasks> Tasks { get; set; } = new List<Tasks>();
    }
}
