using STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace STUDENTPROJECTMANAGEMENTSYSTEM.Models
{
    [Table("SPM_Role")]
    public class Role : BaseModel
    {
        [Key]
        public int RoleId { get; set; }

        [Required]
        [StringLength(50)]
        public string RoleName { get; set; }

        [StringLength(250)]
        public string? Description { get; set; }
    }
}
