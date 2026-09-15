using FluentValidation;
using STUDENTPROJECTMANAEMENTSYSTEMBACKEND.DTOs.ProjectMaster;

namespace STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Validators.ProjectMasterValidator
{
    public class ProjectMasterCreateEditValidator : AbstractValidator<ProjectMasterCreateEditDto>
    {
        public ProjectMasterCreateEditValidator()
        {
            RuleFor(x => x.ProjectTitle)
                .NotEmpty().WithMessage("Project Title is required.")
                .Length(2, 150).WithMessage("Project Title must be between 2 and 150 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");
        }
    }
}
