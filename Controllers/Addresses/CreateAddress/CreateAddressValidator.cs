﻿using FluentValidation;

namespace Ecommerce.Controllers.Addresses.CreateAddress
{
    public class CreateAddressValidator : AbstractValidator<CreateAddressRequest>
    {
        public CreateAddressValidator()
        {
            RuleFor(x => x.Recipient)
                .NotEmpty().WithMessage("Field {PropertyName} is required")
                .Matches(@"^[A-Za-zÀ-ÖØ-öø-ÿ ]*$").WithMessage("Field {PropertyName} is not in a valid format");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Field {PropertyName} is required")
                .Length(10).WithMessage("Field {PropertyName} must contain 10 characters")
                .Matches(@"^[0-9]*$").WithMessage("Field {PropertyName} is not in a valid format");

            RuleFor(x => x.Street)
                .NotEmpty().WithMessage("Field {PropertyName} is required");

            RuleFor(x => x.StreetNumber)
                .NotEmpty().WithMessage("Field {PropertyName} is required")
                .Matches(@"^[A-Za-z0-9]*$").WithMessage("Field {PropertyName} is not in a valid format");

            RuleFor(x => x.AptNumber)
                .Matches(@"^[0-9]*$").WithMessage("Field {PropertyName} is not in a valid format");

            RuleFor(x => x.Neighbourhood)
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
