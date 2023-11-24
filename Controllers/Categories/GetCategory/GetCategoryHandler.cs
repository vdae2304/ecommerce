using Ecommerce.Common.Exceptions;
using Ecommerce.Common.Interfaces;
using Ecommerce.Common.Models.Responses;
using Ecommerce.Common.Models.Schema;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Controllers.Categories.GetCategory
{
    public record GetCategoryRequest : IRequest<ActionResult>
    {
        /// <summary>
        /// Category ID.
        /// </summary>
        [Required]
        public int CategoryId { get; set; } 
    }

    public class GetCategoryHandler : IRequestHandler<GetCategoryRequest, ActionResult>
    {
        private readonly IGenericRepository<Category> _categories;
        private readonly ILogger<GetCategoryHandler> _logger;

        public GetCategoryHandler(IGenericRepository<Category> categories, ILogger<GetCategoryHandler> logger)
        {
            _categories = categories;
            _logger = logger;
        }

        public async Task<ActionResult> Handle(GetCategoryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                Category category = await _categories.AsQueryable()
                    .Include(x => x.Thumbnail)
                    .Include(x => x.Subcategories)
                    .AsSplitQuery()
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == request.CategoryId, cancellationToken)
                    ?? throw new NotFoundException($"Category {request.CategoryId} does not exist");

                return new OkObjectResult(new DataResponse<Category>
                {
                    Success = true,
                    Message = "Ok.",
                    Data = category
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in getting details for category {categoryId}", request.CategoryId);
                throw;
            }
        }
    }
}
