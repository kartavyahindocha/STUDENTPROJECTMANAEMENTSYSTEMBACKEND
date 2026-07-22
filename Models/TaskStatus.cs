using STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Models
{
    
    [Table("SPM_TaskStatus")]
    public class TaskStatusMaster : BaseModel
    {
        [Key]
        public int TaskStatusID { get; set; }

        [Required]
        [StringLength(20)]
        public string TaskStatusName { get; set; }

        [Required]
        [StringLength(100)]
        public string TaskStatusCssClass { get; set; }


        public ICollection<Tasks> Tasks { get; set; } = new List<Tasks>();
    }
    public class TaskStatusMasterDTO
    {
        public int TaskStatusID { get; set; }
        public string TaskStatusName { get; set; }
        public string TaskStatusCssClass{get; set;}
        
    }
}
