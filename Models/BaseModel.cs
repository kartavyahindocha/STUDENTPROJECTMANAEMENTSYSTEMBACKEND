using System.ComponentModel.DataAnnotations;

namespace STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Models
{
    public abstract class BaseModel
    {
        [Required]
        public int CreatedBy { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }
    }
}
