using FluentValidation;
using STUDENTPROJECTMANAEMENTSYSTEMBACKEND.DTOs.Tasks;

namespace STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Validators.TasksValidator
{
    public class TasksCreateEditValidator : AbstractValidator<TasksCreateEditDto>
    {
        public TasksCreateEditValidator()
        {
            RuleFor(x => x.TaskTitle)
                .NotEmpty().WithMessage("Task Title is required.")
                .Length(2, 150).WithMessage("Task Title must be between 2 and 150 characters.");

            RuleFor(x => x.AssignedScore)
                .GreaterThanOrEqualTo(0).WithMessage("Assigned Score must be non-negative.");

            RuleFor(x => x.ProgressPercentage)
                .InclusiveBetween(0, 100).WithMessage("Progress Percentage must be between 0 and 100.");

            RuleFor(x => x.ProjectAllocationID)
                .GreaterThan(0).WithMessage("Valid Project Allocation ID is required.");

            RuleFor(x => x.TaskStatusID)
                .GreaterThan(0).WithMessage("Valid Task Status ID is required.");

            RuleFor(x => x.TaskPriorityID)
                .GreaterThan(0).WithMessage("Valid Task Priority ID is required.");
        }
    }
}
