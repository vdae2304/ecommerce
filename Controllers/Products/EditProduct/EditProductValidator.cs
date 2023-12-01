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

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Field {PropertyName} is required")
                .MaximumLength(128).WithMessage("Field {PropertyName} cannot have more than {MaxLength} characters")
                .Matches(@"^[A-Za-zÀ-ÖØ-öø-ÿ0-9 ]+$").WithMessage("Field {PropertyName} is not in a valid format");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Field {PropertyName} is required");

            RuleFor(x => x.Price)
                .NotEmpty().WithMessage("Field {PropertyName} is required")
                .GreaterThanOrEqualTo(0).WithMessage("Field {PropertyName} cannot be negative");

            RuleFor(x => x.Price)
                .LessThan(x => x.CrossedOutPrice)
                .WithMessage("Field {PropertyName} must be less than {ComparisonProperty}")
                .When(x => x.CrossedOutPrice != null);

            RuleForEach(x => x.CategoryIds)
                .MustAsync(CategoryExists).WithMessage("Category {PropertyValue} does not exist");

            RuleForEach(x => x.Attributes).SetValidator(new CreateAttributeValidator());

            RuleFor(x => x.Length)
                .GreaterThanOrEqualTo(0).WithMessage("Field {PropertyName} cannot be negative");

            RuleFor(x => x.Width)
                .GreaterThanOrEqualTo(0).WithMessage("Field {PropertyName} cannot be negative");

            RuleFor(x => x.Height)
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
                .GreaterThanOrEqualTo(0).WithMessage("Field {PropertyName} cannot be negative");

            RuleFor(x => x.MaxPurchaseQuantity)
                .GreaterThanOrEqualTo(x => x.MinPurchaseQuantity)
                .WithMessage("Field {PropertyName} must be greater than or equal to {ComparisonProperty}");

            RuleFor(x => x.InStock)
                .GreaterThanOrEqualTo(0).WithMessage("Field {PropertyName} cannot be negative");
        }

        private async Task<bool> CategoryExists(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Categories.AnyAsync(x => x.Id == id, cancellationToken);
        }
    }
}
