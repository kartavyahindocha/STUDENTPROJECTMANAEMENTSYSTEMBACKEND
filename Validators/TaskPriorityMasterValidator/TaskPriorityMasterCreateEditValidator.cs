using FluentValidation;
using STUDENTPROJECTMANAEMENTSYSTEMBACKEND.DTOs.TaskPriorityMaster;

namespace STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Validators.TaskPriorityMasterValidator
{
    public class TaskPriorityMasterCreateEditValidator : AbstractValidator<TaskPriorityMasterCreateEditDto>
    {
        public TaskPriorityMasterCreateEditValidator()
        {
            RuleFor(x => x.TaskPriorityName)
                .NotEmpty().WithMessage("Task Priority Name is required.")
                .Length(2, 50).WithMessage("Task Priority Name must be between 2 and 50 characters.");

            RuleFor(x => x.TaskPriortyCssClass)
                .NotEmpty().WithMessage("CSS Class is required.")
                .MaximumLength(50).WithMessage("CSS Class cannot exceed 50 characters.");
        }
    }
}
