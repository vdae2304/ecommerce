using Ecommerce.Common.Interfaces;
using Ecommerce.Common.Models.Schema;
using FluentValidation;

namespace Ecommerce.Controllers.Products.CreateProduct
{
    public class CreateProductValidator : AbstractValidator<CreateProductForm>
    {
        private readonly IGenericRepository<Category> _categories;
        private readonly IGenericRepository<Product> _products;
        private readonly IGenericRepository<MeasureUnit> _units;

        public CreateProductValidator(IGenericRepository<Category> categories, IGenericRepository<Product> products,
            IGenericRepository<MeasureUnit> units)
        {
            _categories = categories;
            _products = products;
            _units = units;
            
            RuleFor(x => x.Sku)
                .NotEmpty().WithMessage("Field {PropertyName} is required")
                .Length(6, 12).WithMessage("Field {PropertyName} must contain between {MinLength} and {MaxLength} characters")
                .Matches(@"^[A-z][A-Z0-9]*$").WithMessage("Field {PropertyName} is not in a valid format")
                .MustAsync(IsSkuUniqueAsync).WithMessage("Field {PropertyName} must be unique");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Field {PropertyName} is required")
                .MaximumLength(128).WithMessage("Field {PropertyName} cannot have more than {MaxLength} characters")
                .Matches(@"^[A-Za-zÀ-ÖØ-öø-ÿ0-9 ]+$").WithMessage("Field {PropertyName} is not in a valid format");

            RuleFor(x => x.Description)
                .NotNull().WithMessage("Field {PropertyName} is required");

            RuleFor(x => x.Price)
                .NotNull().WithMessage("Field {PropertyName} is required")
                .GreaterThanOrEqualTo(0).WithMessage("Field {PropertyName} cannot be negative");

            RuleForEach(x => x.CategoryIds)
                .MustAsync(CategoryExists).WithMessage("Category {PropertyValue} does not exist");

            RuleForEach(x => x.Attributes).SetValidator(new CreateAttributeValidator());

            RuleFor(x => x.Width)
                .GreaterThanOrEqualTo(0).WithMessage("Field {PropertyName} cannot be negative");

            RuleFor(x => x.Height)
                .GreaterThanOrEqualTo(0).WithMessage("Field {PropertyName} cannot be negative");

            RuleFor(x => x.Length)
                .GreaterThanOrEqualTo(0).WithMessage("Field {PropertyName} cannot be negative");

            RuleFor(x => x.DimensionUnitsId)
                .NotNull().WithMessage("Field {PropertyName} is required")
                .MustAsync(DimensionUnitsExists).WithMessage("Field {PropertyName} is not valid")
                .When(x => x.Width != null || x.Height != null || x.Length != null);

            RuleFor(x => x.Weight)
                .GreaterThanOrEqualTo(0).WithMessage("Field {PropertyName} cannot be negative");

            RuleFor(x => x.WeightUnitsId)
                .NotNull().WithMessage("Field {PropertyName} is required")
                .MustAsync(WeightUnitsExists).WithMessage("Field {PropertyName} is not valid")
                .When(x => x.Weight != null);

            RuleFor(x => x.Volume)
                .GreaterThanOrEqualTo(0).WithMessage("Field {PropertyName} cannot be negative");

            RuleFor(x => x.VolumeUnitsId)
                .NotNull().WithMessage("Field {PropertyName} is required")
                .MustAsync(VolumeUnitsExists).WithMessage("Field {PropertyName} is not valid")
                .When(x => x.Volume != null);

            RuleFor(x => x.MinPurchaseQuantity)
                .GreaterThanOrEqualTo(0).WithMessage("Field {PropertyName} cannot be negative");

            RuleFor(x => x.MaxPurchaseQuantity)
                .GreaterThanOrEqualTo(x => x.MinPurchaseQuantity ?? 0)
                .WithMessage("Field {PropertyName} must be greater than or equal to {ComparisonValue}");

            RuleFor(x => x.InStock)
                .GreaterThanOrEqualTo(0).WithMessage("Field {PropertyName} cannot be negative");
        }

        private async Task<bool> IsSkuUniqueAsync(string sku, CancellationToken cancellationToken = default)
        {
            return !await _products.AnyAsync(x => x.Sku == sku, cancellationToken);
        }

        private async Task<bool> CategoryExists(int id, CancellationToken cancellationToken = default)
        {
            return await _categories.AnyAsync(x => x.Id == id, cancellationToken);
        }

        private async Task<bool> DimensionUnitsExists(int? id, CancellationToken cancellationToken = default)
        {
            if (id == null) return true;
            return await _units.AnyAsync(x => x.Id == id && x.Type == MeasureUnitType.Dimension, cancellationToken);
        }

        private async Task<bool> WeightUnitsExists(int? id, CancellationToken cancellationToken = default)
        {
            if (id == null) return true;
            return await _units.AnyAsync(x => x.Id == id && x.Type == MeasureUnitType.Weight, cancellationToken);
        }

        private async Task<bool> VolumeUnitsExists(int? id, CancellationToken cancellationToken = default)
        {
            if (id == null) return true;
            return await _units.AnyAsync(x => x.Id == id && x.Type == MeasureUnitType.Volume, cancellationToken);
        }
    }

    public class CreateAttributeValidator : AbstractValidator<CreateAttributeForm>
    {
        public CreateAttributeValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Field Attribute.{PropertyName} is required")
                .MaximumLength(128).WithMessage("Field Attribute.{PropertyName} cannot have more than {MaxLength} characters")
                .Matches(@"^[A-Za-zÀ-ÖØ-öø-ÿ ]+$").WithMessage("Field Attribute.{PropertyName} is not in a valid format");
        }
    }
}
