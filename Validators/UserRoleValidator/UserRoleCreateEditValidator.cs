using FluentValidation;
using STUDENTPROJECTMANAEMENTSYSTEMBACKEND.DTOs.UserRole;

namespace STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Validators.UserRoleValidator
{
    public class UserRoleCreateEditValidator : AbstractValidator<UserRoleCreateEditDto>
    {
        public UserRoleCreateEditValidator()
        {
            RuleFor(x => x.RoleID)
                .GreaterThan(0).WithMessage("Valid Role ID is required.");

            RuleFor(x => x.UserID)
                .GreaterThan(0).WithMessage("Valid User ID is required.");
        }
    }
}
