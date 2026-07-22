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

        [StringLength(1)]
        public string? OverAllGrade { get; set; }


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

        public ICollection<Tasks> Tasks { get; set; } = new List<Tasks>();
    }
}
