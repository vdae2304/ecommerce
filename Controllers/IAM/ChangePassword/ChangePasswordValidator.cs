using FluentValidation;

namespace Ecommerce.Controllers.IAM.ChangePassword
{
    public class ChangePasswordValidator : AbstractValidator<ChangePasswordRequest>
    {
        public ChangePasswordValidator()
        {
            RuleFor(x => x.CurrentPassword)
                .NotEmpty().WithMessage("Field {PropertyName} is required");

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("Field {PropertyName} is required")
                .Length(8, 32).WithMessage("Field {PropertyName} must contain between {MinLength} and {MaxLength} characters")
                .Matches(@"[A-z]+").WithMessage("Field {PropertyName} must contain at least one uppercase letter")
                .Matches(@"[a-z]+").WithMessage("Field {PropertyName} must contain at least one lowercase letter")
                .Matches(@"[0-9]+").WithMessage("Field {PropertyName} must contain at least one digit")
                .Matches(@"[@!#$%&?*\+\.\-_]+").WithMessage("Field {PropertyName} must contain at least one special character");
        }
    }
}
