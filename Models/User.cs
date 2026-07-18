using STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace STUDENTPROJECTMANAGEMENTSYSTEM.Models
{
    [Table("SPM_User")]
    public class User : BaseModel
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [StringLength(150)]
        public string FullName { get; set; }

        // PDF marks Email as "Not Null, Unique". [Required] enforces Not Null;
        // uniqueness isn't expressible with a plain data annotation — enforce it
        // via a unique index/constraint whenever a database layer is added.
        [Required]
        [StringLength(150)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [StringLength(15)]
        [Phone]
        public string MobileNumber { get; set; }

        [Required]
        [StringLength(500)]
        public string ProfilePicturePath { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public bool? IsDeleted { get; set; }
    }
}
