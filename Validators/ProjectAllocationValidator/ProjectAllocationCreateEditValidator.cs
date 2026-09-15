using FluentValidation;
using STUDENTPROJECTMANAEMENTSYSTEMBACKEND.DTOs.ProjectAllocation;

namespace STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Validators.ProjectAllocationValidator
{
    public class ProjectAllocationCreateEditValidator : AbstractValidator<ProjectAllocationCreateEditDto>
    {
        public ProjectAllocationCreateEditValidator()
        {
            RuleFor(x => x.ProjectID)
                .GreaterThan(0).WithMessage("Valid Project ID is required.");

            RuleFor(x => x.StudentID)
                .GreaterThan(0).WithMessage("Valid Student ID is required.");

            RuleFor(x => x.FacultyID)
                .GreaterThan(0).WithMessage("Valid Faculty ID is required.");

            RuleFor(x => x.ProgressPercentage)
                .InclusiveBetween(0, 100).WithMessage("Progress percentage must be between 0 and 100.");

            RuleFor(x => x.ProjectEndDate)
                .GreaterThanOrEqualTo(x => x.ProjectStartDate).WithMessage("Project End Date cannot be earlier than Project Start Date.");
        }
    }
}
