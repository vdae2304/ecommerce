using FluentValidation;

namespace Ecommerce.Controllers.IAM.Email
{
    public class ConfirmEmailValidator : AbstractValidator<ConfirmEmailRequest>
    {
        public ConfirmEmailValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Field {PropertyName} is required")
                .EmailAddress().WithMessage("Field {PropertyName} is not a valid email address");

            RuleFor(x => x.Token)
                .NotEmpty().WithMessage("Field {PropertyName} is required");
        }
    }
}
