using STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Models
{
    [Table("SPM_ProjectMaster")]
    public class ProjectMaster : BaseModel
    {
        [Key]
        public int ProjectID { get; set; }

        [Required]
        [StringLength(200)]
        public string ProjectTitle { get; set; }

        public string? Description { get; set; }

        // ---- Relationships ----

        // One ProjectMaster -> Many ProjectAllocation (1:N). A single project
        // definition can be allocated to multiple student/faculty pairs.
        public ICollection<ProjectAllocation> ProjectAllocations { get; set; } = new List<ProjectAllocation>();
    }
}
