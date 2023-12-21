using FluentValidation;

namespace Ecommerce.Controllers.Payment.EditPaymentMethod
{
    public class EditPaymentMethodValidator : AbstractValidator<EditPaymentMethodRequest>
    {
        public EditPaymentMethodValidator()
        {
            RuleFor(x => x.Name)
                .Matches(@"^[A-Za-zÀ-ÖØ-öø-ÿ ]*$").WithMessage("Field {PropertyName} is not in a valid format");

            RuleFor(x => x.CVV)
                .Length(3).WithMessage("Field {PropertyName} must contain 3 characters")
                .Matches(@"^[0-9]*$").WithMessage("Field {PropertyName} must contain digits only");

            RuleFor(x => x.ExpiryMonth)
                .NotEmpty().WithMessage("Field {PropertyName} is required")
                .When(x => x.ExpiryYear != null)
                .InclusiveBetween(1, 12).WithMessage("Field {PropertyName} must be between 1 and 12")
                .GreaterThanOrEqualTo(DateTime.Today.Month)
                    .WithMessage("Field {PropertyName} cannot be less than {ComparisonValue}")
                .When(x => x.ExpiryYear == DateTime.Today.Year);

            RuleFor(x => x.ExpiryYear)
                .NotEmpty().WithMessage("Field {PropertyName} is required")
                .When(x => x.ExpiryMonth != null)
                .GreaterThanOrEqualTo(DateTime.Today.Year)
                    .WithMessage("Field {PropertyName} cannot be less than {ComparisonValue}");
        }
    }
}
