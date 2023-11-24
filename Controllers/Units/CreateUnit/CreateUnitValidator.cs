using Ecommerce.Common.Interfaces;
using Ecommerce.Common.Models.Schema;
using FluentValidation;

namespace Ecommerce.Controllers.Units.CreateUnit
{
    public class CreateUnitValidator : AbstractValidator<CreateUnitForm>
    {
        private readonly IGenericRepository<MeasureUnit> _units;

        public CreateUnitValidator(IGenericRepository<MeasureUnit> units)
        {
            _units = units;

            RuleFor(x => x.Symbol)
                .NotEmpty().WithMessage("Field {PropertyName} is required")
                .MaximumLength(8).WithMessage("Field {PropertyName} cannot have more than {MaxLength} characters")
                .Matches(@"^[A-za-z0-9]*$").WithMessage("Field {PropertyName} is not in a valid format")
                .MustAsync(IsSymbolUniqueAsync).WithMessage("Field {PropertyName} must be unique");

            RuleFor(x => x.Type)
                .NotNull().WithMessage("Field {PropertyName} is required")
                .IsInEnum().WithMessage("Field {PropertyName} is not valid");
        }

        private async Task<bool> IsSymbolUniqueAsync(string symbol, CancellationToken cancellationToken = default)
        {
            return !await _units.AnyAsync(x => x.Symbol == symbol, cancellationToken);
        }
    }
}
