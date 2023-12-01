using Ecommerce.Common.Exceptions;
using Ecommerce.Common.Models.Responses;
using Ecommerce.Common.Models.Schema;
using Ecommerce.Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Controllers.Categories.GetCategory
{
    public record GetCategoryRequest : IRequest<IActionResult>
    {
        /// <summary>
        /// Category ID.
        /// </summary>
        public int CategoryId { get; set; } 
    }

    public class GetCategoryHandler : IRequestHandler<GetCategoryRequest, IActionResult>
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<GetCategoryHandler> _logger;

        public GetCategoryHandler(ApplicationDbContext context, ILogger<GetCategoryHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Handle(GetCategoryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                Category category = await _context.Categories
                    .Include(x => x.Thumbnail)
                    .Include(x => x.Subcategories)
                    .ThenInclude(x => x.Thumbnail)
                    .AsSplitQuery()
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == request.CategoryId, cancellationToken)
                    ?? throw new NotFoundException($"Category {request.CategoryId} does not exist");

                return new OkObjectResult(new Response<Category>
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
