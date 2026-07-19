using STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Models
{
    [Table("SPM_UserType")]
    public class UserType : BaseModel
    {
        [Key]
        public int UserTypeID { get; set; }

        // e.g. "Admin", "Student", "Faculty"
        [Required]
        [StringLength(50)]
        public string UserTypeName { get; set; }

        [StringLength(250)]
        public string? Description { get; set; }

        // ---- Relationships ----

        // One UserType -> Many User (1:N). Every user (Admin/Student/Faculty)
        // is classified through this lookup table.
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
