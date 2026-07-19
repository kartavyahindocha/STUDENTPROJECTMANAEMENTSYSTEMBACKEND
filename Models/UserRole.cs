using STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Models
{
    [Table("SPM_UserRole")]
    public class UserRole : BaseModel
    {
        [Key]
        public int RolePermissionID { get; set; }

        // ---- Foreign Keys ----
        // [ForeignKey("<IdColumnName>")] is placed on the navigation property
        // and points at the actual FK id column declared just above it.

        [Required]
        public int RoleID { get; set; }

        [ForeignKey("RoleID")]
        public Role? Role { get; set; }

        [Required]
        public int UserID { get; set; }

        [ForeignKey("UserID")]
        public User? User { get; set; }
    }
}
