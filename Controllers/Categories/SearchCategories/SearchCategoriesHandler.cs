using Ecommerce.Common.Models.Responses;
using Ecommerce.Common.Models.Schema;
using Ecommerce.Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Controllers.Categories.SearchCategories
{
    public class SearchCategoriesHandler : IRequestHandler<SearchCategoriesRequest, IActionResult>
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<SearchCategoriesHandler> _logger;

        public SearchCategoriesHandler(ApplicationDbContext context, ILogger<SearchCategoriesHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Handle(SearchCategoriesRequest filters, CancellationToken cancellationToken)
        {
            try
            {
                var query = _context.Categories
                    .Include(x => x.Thumbnail)
                    .AsSplitQuery()
                    .AsNoTracking();

                if (filters.Keyword != null)
                {
                    query = query.Where(x => x.Name != null && x.Name.Contains(filters.Keyword));
                }
                if (filters.Parent != null)
                {
                    int? parent = (filters.Parent != 0) ? filters.Parent : null;
                    query = query.Where(x => x.ParentId == parent);
                }
                if (filters.Enabled != null)
                {
                    query = query.Where(x => x.Enabled == filters.Enabled);
                }

                int total = await query.CountAsync(cancellationToken);
                List<Category> categories = await query
                    .OrderBy(x => x.Name)
                    .Skip(filters.Offset)
                    .Take(filters.Limit)
                    .ToListAsync(cancellationToken);
                
                return new OkObjectResult(new Response<SearchItems<Category>>
                {
                    Success = true,
                    Message = "Ok.",
                    Data = new SearchItems<Category>
                    {
                        Total = total,
                        Offset = filters.Offset,
                        Limit = filters.Limit,
                        Items = categories
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in searching categories");
                throw;
            }
        }
    }
}
