using STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace STUDENTPROJECTMANAGEMENTSYSTEM.Models
{
    [Table("SPM_Priority")]
    public class Priority : BaseModel
    {
        [Key]
        public int PriorityID { get; set; }

        [Required]
        [StringLength(20)]
        public string PriorityName { get; set; }

        // Used both as the visual "color" (a Bootstrap/utility CSS class, e.g.
        // "tone-danger") and to render badges consistently wherever a Priority is shown.
        [Required]
        [StringLength(20)]
        public string PriorityCssClass { get; set; }
    }
}
