using FluentValidation;
using STUDENTPROJECTMANAEMENTSYSTEMBACKEND.DTOs.TaskStatusMaster;

namespace STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Validators.TaskStatusMasterValidator
{
    public class TaskStatusMasterCreateEditValidator : AbstractValidator<TaskStatusMasterCreateEditDto>
    {
        public TaskStatusMasterCreateEditValidator()
        {
            RuleFor(x => x.TaskStatusName)
                .NotEmpty().WithMessage("Task Status Name is required.")
                .Length(2, 50).WithMessage("Task Status Name must be between 2 and 50 characters.");

            RuleFor(x => x.TaskStatusCssClass)
                .NotEmpty().WithMessage("CSS Class is required.")
                .MaximumLength(50).WithMessage("CSS Class cannot exceed 50 characters.");
        }
    }
}
