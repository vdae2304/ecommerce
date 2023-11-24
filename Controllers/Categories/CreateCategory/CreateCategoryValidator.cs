using Ecommerce.Common.Interfaces;
using Ecommerce.Common.Models.Schema;
using FluentValidation;

namespace Ecommerce.Controllers.Categories.CreateCategory
{
    public class CreateCategoryValidator : AbstractValidator<CreateCategoryForm>
    {
        private readonly IGenericRepository<Category> _categories;
        private readonly IGenericRepository<Product> _products;

        public CreateCategoryValidator(IGenericRepository<Category> categories, IGenericRepository<Product> products)
        {
            _categories = categories;
            _products = products;

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
            return await _categories.AnyAsync(x => x.Id == id, cancellationToken);
        }

        private async Task<bool> ProductExists(int id, CancellationToken cancellationToken)
        {
            return await _products.AnyAsync(x => x.Id == id, cancellationToken);
        }
    }
}
