using Ecommerce.Common.Exceptions;
using Ecommerce.Common.Interfaces;
using Ecommerce.Common.Models.Responses;
using Ecommerce.Common.Models.Schema;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Controllers.Categories.DeleteCategory
{
    public record DeleteCategoryRequest : IRequest<ActionResult>
    {
        /// <summary>
        /// Category ID.
        /// </summary>
        [Required]
        public int CategoryId { get; set; }
    }

    public class DeleteCategoryHandler : IRequestHandler<DeleteCategoryRequest, ActionResult>
    {
        private readonly IGenericRepository<Category> _categories;
        private readonly ILogger<DeleteCategoryHandler> _logger;

        public DeleteCategoryHandler(IGenericRepository<Category> categories, ILogger<DeleteCategoryHandler> logger)
        {
            _categories = categories;
            _logger = logger;
        }

        public async Task<ActionResult> Handle(DeleteCategoryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                Category category = await _categories.FindByIdAsync(request.CategoryId, cancellationToken)
                    ?? throw new NotFoundException($"Category {request.CategoryId} does not exist");

                await _categories.DeleteAsync(category, cancellationToken);

                return new OkObjectResult(new StatusResponse
                {
                    Success = true,
                    Message = "Ok."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in deleting category {categoryId}", request.CategoryId);
                throw;
            }
        }
    }
}
