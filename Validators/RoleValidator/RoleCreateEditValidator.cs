using FluentValidation;
using STUDENTPROJECTMANAEMENTSYSTEMBACKEND.DTOs.Role;
using System.Linq;

namespace STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Validators.RoleValidator
{
    public class RoleCreateEditValidator : AbstractValidator<RoleCreateEditDto>
    {
        public RoleCreateEditValidator()
        {
            RuleFor(x => x.RoleName)
                .NotEmpty().WithMessage("Role Name is required.")
                .Must(name => !string.IsNullOrWhiteSpace(name)).WithMessage("Role Name cannot be empty or whitespace.")
                .Must(name => !name.Any(char.IsDigit)).WithMessage("Role Name cannot contain digits.")
                .Length(2, 50).WithMessage("Role Name must be between 2 and 50 characters.");

            RuleFor(x => x.Description)
                .Must(desc => desc == null || !desc.Any(char.IsDigit)).WithMessage("Role Description cannot contain digits.")
                .MaximumLength(250).WithMessage("Role Description cannot exceed 250 characters.");
        }
    }
}
