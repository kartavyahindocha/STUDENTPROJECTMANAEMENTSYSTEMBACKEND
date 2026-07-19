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

        // ---- Relationships ----

        // One User -> Many UserRole (1:N). Paired with Role.UserRoles, this is
        // the N:N between User and Role via the SPM_UserRole join entity.
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

        // One User -> Many ProjectAllocation, as the assigned Student (1:N).
        // Split into two collections (rather than one) because a User can
        // appear on the same ProjectAllocation row twice — once as Student,
        // once as Faculty — so each FK needs its own named inverse.
        public ICollection<ProjectAllocation> ProjectAllocationsAsStudent { get; set; } = new List<ProjectAllocation>();

        // One User -> Many ProjectAllocation, as the supervising Faculty (1:N).
        public ICollection<ProjectAllocation> ProjectAllocationsAsFaculty { get; set; } = new List<ProjectAllocation>();
    }
}
