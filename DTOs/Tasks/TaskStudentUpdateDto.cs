using System;
using System.ComponentModel.DataAnnotations;

namespace STUDENTPROJECTMANAEMENTSYSTEMBACKEND.DTOs.Tasks
{
    public class TaskStudentUpdateDto
    {
        [Required]
        public int TaskID { get; set; }

        [Required]
        [Range(0, 100, ErrorMessage = "Progress percentage must be between 0 and 100.")]
        public decimal ProgressPercentage { get; set; }

        [StringLength(500)]
        public string? StudentRemarks { get; set; }

        public int TaskStatusID { get; set; }

        public DateTime? TaskCompletedDate { get; set; }
    }
}
