using STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Models;
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
        public string FullName { get; set; }

        // EnrollmentNo, Faculty Code, etc. — optional, format depends on UserType.
        [StringLength(100)]
        public string? UserCode { get; set; }

        // Not Null, Unique — uniqueness enforced via HasIndex(...).IsUnique() in AppDbContext.
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

        // ---- Foreign Keys ----

        [Required]
        public int UserTypeID { get; set; }

        [ForeignKey("UserTypeID")]
        public UserType? UserType { get; set; }


        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

        public ICollection<ProjectAllocation> ProjectAllocationsAsStudent { get; set; } = new List<ProjectAllocation>();

        
        public ICollection<ProjectAllocation> ProjectAllocationsAsFaculty { get; set; } = new List<ProjectAllocation>();
    }
}
