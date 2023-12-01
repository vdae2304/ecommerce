using Ecommerce.Controllers.Products.CreateProduct;
using Ecommerce.Infrastructure.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Controllers.Products.EditProduct
{
    public class EditProductValidator : AbstractValidator<EditProductForm>
    {
        private readonly ApplicationDbContext _context;

        public EditProductValidator(ApplicationDbContext context)
        {
            _context = context;

            RuleFor(x => x.Sku)
                .Length(6, 12).WithMessage("Field {PropertyName} must contain between {MinLength} and {MaxLength} characters")
                .Matches(@"^[A-z][A-Z0-9]*$").WithMessage("Field {PropertyName} is not in a valid format")
                .MustAsync(IsSkuUniqueAsync).WithMessage("Field {PropertyName} must be unique");

            RuleFor(x => x.Name)
                .MaximumLength(128).WithMessage("Field {PropertyName} cannot have more than {MaxLength} characters")
                .Matches(@"^[A-Za-zÀ-ÖØ-öø-ÿ0-9 ]+$").WithMessage("Field {PropertyName} is not in a valid format");

            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0).WithMessage("Field {PropertyName} cannot be negative");

            RuleFor(x => x.CrossedOutPrice)
                .GreaterThanOrEqualTo(0).WithMessage("Field {PropertyName} cannot be negative");

            RuleFor(x => x.Price)
                .NotNull().WithMessage("Field {PropertyName} is required")
                .LessThan(x => x.CrossedOutPrice).WithMessage("Field {PropertyName} must be less than {ComparisonProperty}")
                .When(x => x.CrossedOutPrice != null);

            RuleFor(x => x.Width)
                .GreaterThanOrEqualTo(0).WithMessage("Field {PropertyName} cannot be negative");

            RuleFor(x => x.Height)
                .GreaterThanOrEqualTo(0).WithMessage("Field {PropertyName} cannot be negative");

            RuleFor(x => x.Length)
                .GreaterThanOrEqualTo(0).WithMessage("Field {PropertyName} cannot be negative");

            RuleFor(x => x.DimensionUnits)
                .NotNull().WithMessage("Field {PropertyName} is required")
                .IsInEnum().WithMessage("Field {PropertyName} is not valid")
                .When(x => x.Width != null || x.Height != null || x.Length != null);

            RuleFor(x => x.Weight)
                .GreaterThanOrEqualTo(0).WithMessage("Field {PropertyName} cannot be negative");

            RuleFor(x => x.WeightUnits)
                .NotNull().WithMessage("Field {PropertyName} is required")
                .IsInEnum().WithMessage("Field {PropertyName} is not valid")
                .When(x => x.Weight != null);

            RuleFor(x => x.MinPurchaseQuantity)
                .NotNull().WithMessage("Field {PropertyName} is required")
                .GreaterThanOrEqualTo(0).WithMessage("Field {PropertyName} cannot be negative")
                .When(x => x.MaxPurchaseQuantity != null);

            RuleFor(x => x.MaxPurchaseQuantity)
                .NotNull().WithMessage("Field {PropertyName} is required")
                .GreaterThanOrEqualTo(x => x.MinPurchaseQuantity)
                .WithMessage("Field {PropertyName} must be greater than or equal to {ComparisonProperty}")
                .When(x => x.MinPurchaseQuantity != null);

            RuleFor(x => x.InStock)
                .GreaterThanOrEqualTo(0).WithMessage("Field {PropertyName} cannot be negative");
        }

        private async Task<bool> IsSkuUniqueAsync(string sku, CancellationToken cancellationToken = default)
        {
            return !await _context.Products.AnyAsync(x => x.Sku == sku, cancellationToken);
        }
    }
}
