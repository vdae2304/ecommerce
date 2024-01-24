using Ecommerce.Infrastructure.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Controllers.Categories.EditCategory
{
    public class EditCategoryValidator : AbstractValidator<EditCategoryRequest>
    {
        private readonly ApplicationDbContext _context;

        public EditCategoryValidator(ApplicationDbContext context)
        {
            _context = context;

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Field {PropertyName} is required")
                .MaximumLength(128).WithMessage("Field {PropertyName} cannot have more than {MaxLength} characters");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Field {PropertyName} is required");

            RuleFor(x => x.ParentId)
                .MustAsync(CategoryExistsAsync).WithMessage("Category {PropertyValue} does not exist");

            RuleForEach(x => x.ProductIds)
                .MustAsync(ProductExistsAsync).WithMessage("Product {PropertyValue} does not exist");
        }

        private async Task<bool> CategoryExistsAsync(int? id, CancellationToken cancellationToken)
        {
            return (id == null) || await _context.Categories.AnyAsync(x => x.Id == id, cancellationToken);
        }

        private async Task<bool> ProductExistsAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.Products.AnyAsync(x => x.Id == id, cancellationToken);
        }
    }
}
