using STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace STUDENTPROJECTMANAGEMENTSYSTEM.Models
{
    [Table("SPM_Status")]
    public class Status : BaseModel
    {
        [Key]
        public int StatusID { get; set; }

        [Required]
        [StringLength(20)]
        public string StatusName { get; set; }

        // Used both as the visual "color" (a Bootstrap/utility CSS class, e.g.
        // "tone-success") and to render badges consistently wherever a Status is shown.
        [Required]
        [StringLength(100)]
        public string StatusCssClass { get; set; }
    }
}
