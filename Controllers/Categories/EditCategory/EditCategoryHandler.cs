using Ecommerce.Common.Exceptions;
using Ecommerce.Common.Models.Responses;
using Ecommerce.Common.Models.Schema;
using Ecommerce.Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Ecommerce.Controllers.Categories.EditCategory
{
    public record EditCategoryForm : IRequest<IActionResult>
    {
        /// <summary>
        /// Category ID.
        /// </summary>
        [JsonIgnore]
        public int CategoryId { get; set; }

        /// <summary>
        /// ID of the parent category, if any.
        /// Default is to set as root category.
        /// </summary>
        public int? ParentId { get; set; }

        /// <summary>
        /// Category name.
        /// </summary>
        [Required]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Category description.
        /// </summary>
        [Required]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// ID of the products to assign to the category.
        /// </summary>
        public IEnumerable<int> ProductIds { get; set; } = new List<int>();

        /// <summary>
        /// Whether the category is enabled or not.
        /// Default is true.
        /// </summary>
        public bool Enabled { get; set; } = true;
    }

    public class EditCategoryHandler : IRequestHandler<EditCategoryForm, IActionResult>
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<EditCategoryHandler> _logger;

        public EditCategoryHandler(ApplicationDbContext context, ILogger<EditCategoryHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Handle(EditCategoryForm request, CancellationToken cancellationToken)
        {
            try
            {
                var validator = new EditCategoryValidator(_context);
                var validationResult = await validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    throw new BadRequestException(validationResult.ToString());
                }

                Category category = await _context.Categories
                    .FirstOrDefaultAsync(x => x.Id == request.CategoryId, cancellationToken)
                    ?? throw new NotFoundException($"Category {request.CategoryId} does not exist");

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
