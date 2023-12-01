using Ecommerce.Infrastructure.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Controllers.Categories.EditCategory
{
    public class EditCategoryValidator : AbstractValidator<EditCategoryForm>
    {
        private readonly ApplicationDbContext _context;

        public EditCategoryValidator(ApplicationDbContext context)
        {
            _context = context;

            RuleFor(x => x.Name)
                .MaximumLength(128).WithMessage("Field {PropertyName} cannot have more than {MaxLength} characters")
                .Matches(@"^[A-Za-zÀ-ÖØ-öø-ÿ0-9 ]+$").WithMessage("Field {PropertyName} is not in a valid format");

            RuleFor(x => x.ParentId)
                .MustAsync(CategoryExists).WithMessage("Category {PropertyValue} does not exist");

            RuleForEach(x => x.ProductIds)
                .MustAsync(ProductExists).WithMessage("Product {PropertyValue} does not exist");
        }

        private async Task<bool> CategoryExists(int? id, CancellationToken cancellationToken)
        {
            return (id == null) || (id == 0)
                || await _context.Categories.AnyAsync(x => x.Id == id, cancellationToken);
        }

        private async Task<bool> ProductExists(int id, CancellationToken cancellationToken)
        {
            return await _context.Products.AnyAsync(x => x.Id == id, cancellationToken);
        }
    }
}
