using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Models
{
    [Table("SPM_User")]
    public class User : BaseModel
    {
        [Key]
        public int UserID { get; set; }

        [Required]
        [StringLength(150)]
        public string FullName { get; set; } = string.Empty;

        [StringLength(100)]
        public string? UserCode { get; set; }

        [Required]
        [StringLength(150)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        [Required]
        [StringLength(15)]
        [Phone]
        public string MobileNumber { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string ProfilePicturePath { get; set; } = "/images/default-user.png";

        [Required]
        public bool IsActive { get; set; } = true;

        public bool? IsDeleted { get; set; } = false;

        [Required]
        public int UserTypeID { get; set; }

        [ForeignKey("UserTypeID")]
        public UserType? UserType { get; set; }

        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
        public ICollection<ProjectAllocation> ProjectAllocationsAsStudent { get; set; } = new List<ProjectAllocation>();
        public ICollection<ProjectAllocation> ProjectAllocationsAsFaculty { get; set; } = new List<ProjectAllocation>();
    }

}
