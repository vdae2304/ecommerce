using FluentValidation;

namespace Ecommerce.Controllers.Payment.CreatePaymentMethod
{
    public class CreatePaymentMethodValidator : AbstractValidator<CreatePaymentMethodForm>
    {
        public CreatePaymentMethodValidator()
        {
            RuleFor(x => x.CardOwner)
                .NotEmpty().WithMessage("Field {PropertyName} is required")
                .Matches(@"^[A-Za-zÀ-ÖØ-öø-ÿ ]*$").WithMessage("Field {PropertyName} is not in a valid format");

            RuleFor(x => x.CardNumber)
                .NotEmpty().WithMessage("Field {PropertyName} is required")
                .Length(16).WithMessage("Field {PropertyName} must contain 16 characters")
                .Matches(@"^[0-9]*$").WithMessage("Field {PropertyName} must contain digits only")
                .CreditCard().WithMessage("Field {PropertyName} is not a valid card number");

            RuleFor(x => x.CVV)
                .NotEmpty().WithMessage("Field {PropertyName} is required")
                .Length(3).WithMessage("Field {PropertyName} must contain 3 characters")
                .Matches(@"^[0-9]*$").WithMessage("Field {PropertyName} must contain digits only");

            RuleFor(x => x.ExpiryMonth)
                .NotEmpty().WithMessage("Field {PropertyName} is required")
                .InclusiveBetween(1, 12).WithMessage("Field {PropertyName} must be between 1 and 12")
                .GreaterThanOrEqualTo(DateTime.Today.Month)
                    .WithMessage("Field {PropertyName} cannot be less than {ComparisonValue}")
                .When(x => x.ExpiryYear == DateTime.Today.Year);

            RuleFor(x => x.ExpiryYear)
                .NotEmpty().WithMessage("Field {PropertyName} is required")
                .GreaterThanOrEqualTo(DateTime.Today.Year)
                    .WithMessage("Field {PropertyName} cannot be less than {ComparisonValue}");

            RuleFor(x => x.BillingAddressId)
                .NotNull().WithMessage("Field {PropertyName} is required");
        }
    }
}
