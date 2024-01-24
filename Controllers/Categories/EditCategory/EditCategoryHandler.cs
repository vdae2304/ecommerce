using Ecommerce.Common.Exceptions;
using Ecommerce.Common.Models.Responses;
using Ecommerce.Common.Models.Schema;
using Ecommerce.Infrastructure.Data;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Controllers.Categories.EditCategory
{
    public class EditCategoryHandler : IRequestHandler<EditCategoryRequest, IActionResult>
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<EditCategoryHandler> _logger;

        public EditCategoryHandler(ApplicationDbContext context, ILogger<EditCategoryHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Handle(EditCategoryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var validator = new EditCategoryValidator(_context);
                await validator.ValidateAndThrowAsync(request, cancellationToken);

                Category category = await _context.Categories
                    .FirstOrDefaultAsync(x => x.Id == request.CategoryId, cancellationToken)
                    ?? throw new NotFoundException();

                category.ParentId = request.ParentId;
                category.Name = request.Name;
                category.Description = request.Description;
                category.Enabled = request.Enabled;

                List<ProductCategories> newProducts = request.ProductIds
                    .Select(productId => new ProductCategories
                    {
                        CategoryId = request.CategoryId,
                        ProductId = productId
                    })
                    .ToList();
                _context.ProductCategories.AddRange(newProducts);

                _context.Categories.Update(category);
                await _context.SaveChangesAsync(cancellationToken);

                return new OkObjectResult(new Response
                {
                    Success = true,
                    Message = "Ok."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in updating category {categoryId}", request.Name);
                throw;
            }
        }
    }
}
