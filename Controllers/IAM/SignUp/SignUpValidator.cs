using Ecommerce.Common.Models.IAM;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace Ecommerce.Controllers.IAM.SignUp
{
    public class SignUpValidator : AbstractValidator<SignUpRequest>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public SignUpValidator(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;

            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Field {PropertyName} is required")
                .Length(6, 24).WithMessage("Field {PropertyName} must contain between {MinLength} and {MaxLength} characters")
                .Matches(@"^[A-Za-z0-9_.]*$").WithMessage("Field {PropertyName} is not in a valid format")
                .MustAsync(IsUsernameUnique).WithMessage("{PropertyName} {PropertyValue} is already in use");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Field {PropertyName} is required")
                .Length(8, 32).WithMessage("Field {PropertyName} must contain between {MinLength} and {MaxLength} characters")
                .Matches(@"[A-z]+").WithMessage("Field {PropertyName} must contain at least one uppercase letter")
                .Matches(@"[a-z]+").WithMessage("Field {PropertyName} must contain at least one lowercase letter")
                .Matches(@"[0-9]+").WithMessage("Field {PropertyName} must contain at least one digit")
                .Matches(@"[@!#$%&?*\+\.\-_]+").WithMessage("Field {PropertyName} must contain at least one special character");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Field {PropertyName} is required")
                .EmailAddress().WithMessage("Field {PropertyName} is not a valid email address")
                .MustAsync(IsEmailUnique).WithMessage("{PropertyName} {PropertyValue} is already in use");
        }

        private async Task<bool> IsUsernameUnique(string username, CancellationToken cancellationToken)
        {
            return await _userManager.FindByNameAsync(username) == null;
        }

        private async Task<bool> IsEmailUnique(string email, CancellationToken cancellationToken)
        {
            return await _userManager.FindByEmailAsync(email) == null;
        }
    }
}
