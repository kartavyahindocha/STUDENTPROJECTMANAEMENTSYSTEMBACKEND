using STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Models
{
    [Table("SPM_Role")]
    public class Role : BaseModel
    {
        [Key]
        public int RoleID { get; set; }

        [Required]
        [StringLength(50)]
        public string RoleName { get; set; }

        [StringLength(250)]
        public string? Description { get; set; }

        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
    public class RoleDTO
    {
        public int RoleID { get; set; }
        public string RoleName { get; set; }
        public string? Description { get; set; }
    }   

}
