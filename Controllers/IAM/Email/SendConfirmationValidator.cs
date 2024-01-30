using FluentValidation;

namespace Ecommerce.Controllers.IAM.Email
{
    public class SendConfirmationValidator : AbstractValidator<SendConfirmationRequest>
    {
        public SendConfirmationValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Field {PropertyName} is required")
                .EmailAddress().WithMessage("Field {PropertyName} is not a valid email address");
        }
    }
}
