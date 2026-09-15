using FluentValidation;
using STUDENTPROJECTMANAEMENTSYSTEMBACKEND.DTOs.Tasks;

namespace STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Validators.TasksValidator
{
    public class TaskStudentUpdateValidator : AbstractValidator<TaskStudentUpdateDto>
    {
        public TaskStudentUpdateValidator()
        {
            RuleFor(x => x.ProgressPercentage)
                .InclusiveBetween(0, 100).WithMessage("Progress percentage must be between 0 and 100.");

            RuleFor(x => x.StudentRemarks)
                .MaximumLength(500).WithMessage("Student remarks cannot exceed 500 characters.");
        }
    }
}
