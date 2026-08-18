using FluentValidation;
using STUDENTPROJECTMANAEMENTSYSTEMBACKEND.DTOs.User;

namespace STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Validators.UserValidator
{
    public class UserCreateEditValidator : AbstractValidator<UserCreateEditDto>
    {
        public UserCreateEditValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Full Name is required.")
                .Length(2, 100).WithMessage("Full Name must be between 2 and 100 characters.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("A valid email address is required.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");

            RuleFor(x => x.MobileNumber)
                .NotEmpty().WithMessage("Mobile Number is required.")
                .Matches(@"^\+?[0-9]{7,15}$").WithMessage("Mobile Number must be a valid phone number (7 to 15 digits).");

            RuleFor(x => x.UserTypeID)
                .GreaterThan(0).WithMessage("Valid User Type is required.");
        }
    }
}
