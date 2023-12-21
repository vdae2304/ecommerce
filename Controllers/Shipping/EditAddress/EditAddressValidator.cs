using FluentValidation;

namespace Ecommerce.Controllers.Shipping.EditAddress
{
    public class EditAddressValidator : AbstractValidator<EditAddressRequest>
    {
        public EditAddressValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Field {PropertyName} is required")
                .Matches(@"^[A-Za-zÀ-ÖØ-öø-ÿ ]*$").WithMessage("Field {PropertyName} is not in a valid format");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Field {PropertyName} is required")
                .Length(10).WithMessage("Field {PropertyName} must contain 10 characters")
                .Matches(@"^[0-9]*$").WithMessage("Field {PropertyName} is not in a valid format");

            RuleFor(x => x.Street)
                .NotEmpty().WithMessage("Field {PropertyName} is required");

            RuleFor(x => x.City)
                .NotEmpty().WithMessage("Field {PropertyName} is required");

            RuleFor(x => x.State)
                .NotEmpty().WithMessage("Field {PropertyName} is required");

            RuleFor(x => x.PostalCode)
                .NotEmpty().WithMessage("Field {PropertyName} is required")
                .Length(5).WithMessage("Field {PropertyName} must contain 5 characters")
                .Matches(@"^[0-9]*$").WithMessage("Field {PropertyName} is not in a valid format");
        }
    }
}
