using Ecommerce.Common.Exceptions;
using Ecommerce.Common.Models.Responses;
using Ecommerce.Common.Models.Schema;
using Ecommerce.Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        /// (Optional) ID of the parent category.
        /// </summary>
        public int? ParentId { get; set; } = 0;

        /// <summary>
        /// (Optional) Category name.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// (Optional) Category description.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// (Optional) ID of the products to assign to the category.
        /// </summary>
        public IEnumerable<int> ProductIds { get; set; } = new List<int>();

        /// <summary>
        /// (Optional) Whether the category is enabled or not.
        /// </summary>
        public bool? Enabled { get; set; }
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

                if (request.ParentId != 0)
                {
                    category.ParentId = request.ParentId;
                }
                if (request.Name != null)
                {
                    category.Name = request.Name;
                }
                if (request.Description != null)
                {
                    category.Description = request.Description;
                }
                if (request.Enabled != null)
                {
                    category.Enabled = request.Enabled.Value;
                }

                category.Products = await _context.Products
                    .Where(x => request.ProductIds.Contains(x.Id))
                    .ToListAsync(cancellationToken);

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
