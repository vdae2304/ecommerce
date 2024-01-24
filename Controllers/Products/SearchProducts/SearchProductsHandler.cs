using Ecommerce.Common.Models.Responses;
using Ecommerce.Common.Models.Schema;
using Ecommerce.Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Controllers.Products.SearchProducts
{
    public class SearchProductsHandler : IRequestHandler<SearchProductsRequest, IActionResult>
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<SearchProductsHandler> _logger;

        public SearchProductsHandler(ApplicationDbContext context, ILogger<SearchProductsHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Handle(SearchProductsRequest filters, CancellationToken cancellationToken)
        {
            try
            {
                var query = _context.Products
                    .Include(x => x.Thumbnail)
                    .Include(x => x.Categories)
                    .Include(x => x.Attributes)
                    .AsSplitQuery()
                    .AsNoTracking();

                if (filters.Keyword != null)
                {
                    query = query.Where(x => x.Name != null && x.Name.Contains(filters.Keyword));
                }
                if (filters.Sku.Any())
                {
                    query = query.Where(x => filters.Sku.Contains(x.Sku));
                }
                if (filters.Category != null)
                {
                    query = query.Where(x => x.Categories.Any(category => category.Id == filters.Category));
                }
                if (filters.PriceFrom != null)
                {
                    query = query.Where(x => x.Price >= filters.PriceFrom);
                }
                if (filters.PriceTo != null)
                {
                    query = query.Where(x => x.Price <= filters.PriceTo);
                }
                if (filters.Enabled != null)
                {
                    query = query.Where(x => x.Enabled == filters.Enabled);
                }
                if (filters.CreatedFrom != null)
                {
                    query = query.Where(x => x.CreatedAt >= filters.CreatedFrom);
                }
                if (filters.CreatedTo != null)
                {
                    query = query.Where(x => x.CreatedAt <= filters.CreatedTo);
                }
                if (filters.UpdatedFrom != null)
                {
                    query = query.Where(x => x.UpdatedAt >= filters.UpdatedFrom);
                }
                if (filters.UpdatedTo != null)
                {
                    query = query.Where(x => x.UpdatedAt <= filters.UpdatedTo);
                }

                foreach (var attribute in filters.Attributes)
                {
                    string name = attribute.Key;
                    string? value = attribute.Value;
                    query = query.Where(x => x.Attributes.Any(attribute => attribute.Name == name && attribute.Value == value));
                }

                switch (filters.SortBy)
                {
                    case SortCriteria.NAME_ASC:
                        query = query.OrderBy(x => x.Name);
                        break;
                    case SortCriteria.NAME_DESC:
                        query = query.OrderByDescending(x => x.Name);
                        break;
                    case SortCriteria.PRICE_ASC:
                        query = query.OrderBy(x => x.Price); break;
                    case SortCriteria.PRICE_DESC:
                        query = query.OrderByDescending(x => x.Price);
                        break;
                    case SortCriteria.CREATED_ASC:
                        query = query.OrderBy(x => x.CreatedAt);
                        break;
                    case SortCriteria.CREATED_DESC:
                        query = query.OrderByDescending(x => x.CreatedAt);
                        break;
                    case SortCriteria.UPDATED_ASC:
                        query = query.OrderBy(x => x.UpdatedAt);
                        break;
                    case SortCriteria.UPDATED_DESC:
                        query = query.OrderByDescending(x => x.UpdatedAt);
                        break;
                }

                int total = await query.CountAsync(cancellationToken);
                List<Product> products = await query
                    .Skip(filters.Offset)
                    .Take(filters.Limit)
                    .ToListAsync(cancellationToken);

                return new OkObjectResult(new Response<SearchItems<Product>>
                {
                    Success = true,
                    Message = "Ok.",
                    Data = new SearchItems<Product>
                    {
                        Total = total,
                        Offset = filters.Offset,
                        Limit = filters.Limit,
                        Items = products
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in searching products");
                throw;
            }
        }
    }
}
