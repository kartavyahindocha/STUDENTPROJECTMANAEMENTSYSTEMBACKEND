using STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Models
{
    [Table("SPM_TaskPriority")]
    public class TaskPriorityMaster : BaseModel
    {
        [Key]
        public int TaskPriorityID { get; set; }

        [Required]
        [StringLength(20)]
        public string TaskPriorityName { get; set; }

        [Required]
        [StringLength(20)]
        public string TaskPriortyCssClass { get; set; }


        public ICollection<Tasks> Tasks { get; set; } = new List<Tasks>();
    }
}
