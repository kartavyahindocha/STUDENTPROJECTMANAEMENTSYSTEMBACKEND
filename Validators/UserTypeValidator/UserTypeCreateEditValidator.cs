using FluentValidation;
using STUDENTPROJECTMANAEMENTSYSTEMBACKEND.DTOs.UserType;

namespace STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Validators.UserTypeValidator
{
    public class UserTypeCreateEditValidator : AbstractValidator<UserTypeCreateEditDto>
    {
        public UserTypeCreateEditValidator()
        {
            RuleFor(x => x.UserTypeName)
                .NotEmpty().WithMessage("User Type Name is required.")
                .Length(2, 50).WithMessage("User Type Name must be between 2 and 50 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(250).WithMessage("Description cannot exceed 250 characters.");
        }
    }
}
