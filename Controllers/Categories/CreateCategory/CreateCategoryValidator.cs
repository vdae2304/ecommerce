using Ecommerce.Infrastructure.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Controllers.Categories.CreateCategory
{
    public class CreateCategoryValidator : AbstractValidator<CreateCategoryForm>
    {
        private readonly ApplicationDbContext _context;

        public CreateCategoryValidator(ApplicationDbContext context)
        {
            _context = context;

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Field {PropertyName} is required")
                .MaximumLength(128).WithMessage("Field {PropertyName} cannot have more than {MaxLength} characters")
                .Matches(@"^[A-Za-zÀ-ÖØ-öø-ÿ0-9 ]+$").WithMessage("Field {PropertyName} is not in a valid format");

            RuleFor(x => x.Description)
                .NotNull().WithMessage("Field {PropertyName} is required");

            RuleFor(x => x.ParentId)
                .MustAsync(CategoryExists).WithMessage("Category {PropertyValue} does not exist");

            RuleForEach(x => x.ProductIds)
                .MustAsync(ProductExists).WithMessage("Product {PropertyValue} does not exist");
        }

        private async Task<bool> CategoryExists(int? id, CancellationToken cancellationToken)
        {
            if (id == null) return true;
            return await _context.Categories.AnyAsync(x => x.Id == id, cancellationToken);
        }

        private async Task<bool> ProductExists(int id, CancellationToken cancellationToken)
        {
            return await _context.Products.AnyAsync(x => x.Id == id, cancellationToken);
        }
    }
}
