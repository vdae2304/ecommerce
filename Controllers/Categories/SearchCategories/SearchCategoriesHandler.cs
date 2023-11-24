using Ecommerce.Common.Interfaces;
using Ecommerce.Common.Models.Responses;
using Ecommerce.Common.Models.Schema;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Controllers.Categories.SearchCategories
{
    public record CategoryFilters : IRequest<ActionResult>
    {
        /// <summary>
        /// Categories to search for.
        /// </summary>
        public string? Keyword { get; set; }

        /// <summary>
        /// Search for categories that are a subcategory of this.
        /// Set to 0 to get root categories.
        /// </summary>
        public int? Parent { get; set; }

        /// <summary>
        /// If true, show only enabled categories. If false, show only disabled categories.
        /// </summary>
        public bool? Enabled { get; set; }

        /// <summary>
        /// Number of items to skip at the beginning.
        /// </summary>
        public int Offset { get; set; } = 0;

        /// <summary>
        /// Maximum number of items to return.
        /// </summary>
        public int Limit { get; set; } = 100;
    }

    public class SearchCategoriesHandler : IRequestHandler<CategoryFilters, ActionResult>
    {
        private readonly IGenericRepository<Category> _categories;
        private readonly ILogger<SearchCategoriesHandler> _logger;

        public SearchCategoriesHandler(IGenericRepository<Category> categories, ILogger<SearchCategoriesHandler> logger)
        {
            _categories = categories;
            _logger = logger;
        }

        public async Task<ActionResult> Handle(CategoryFilters filters, CancellationToken cancellationToken)
        {
            try
            {
                var query = _categories.AsQueryable()
                    .Include(x => x.Thumbnail)
                    .Include(x => x.Subcategories)
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
                
                return new OkObjectResult(new DataResponse<SearchItems<Category>>
                {
                    Success = true,
                    Message = "Ok.",
                    Data = new SearchItems<Category> {
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
