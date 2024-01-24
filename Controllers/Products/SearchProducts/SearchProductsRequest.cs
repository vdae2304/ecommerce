using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers.Products.SearchProducts
{
    public record SearchProductsRequest : IRequest<IActionResult>
    {
        /// <summary>
        /// Products to search for.
        /// </summary>
        public string? Keyword { get; set; }

        /// <summary>
        /// Sku to search for.
        /// </summary>
        public IEnumerable<string> Sku { get; set; } = new List<string>();

        /// <summary>
        /// Search for products assigned to this category.
        /// </summary>
        public int? Category { get; set; }

        /// <summary>
        /// Minimum price to search for.
        /// </summary>
        public decimal? PriceFrom { get; set; }

        /// <summary>
        /// Maximum price to search to.
        /// </summary>
        public decimal? PriceTo { get; set; }

        /// <summary>
        /// Attributes to search for.
        /// </summary>
        public IDictionary<string, string?> Attributes { get; set; } = new Dictionary<string, string?>();

        /// <summary>
        /// If true, show only enabled products.
        /// If false, show only disabled products.
        /// </summary>
        public bool? Enabled { get; set; }

        /// <summary>
        /// Search for products created after this date.
        /// </summary>
        public DateTime? CreatedFrom { get; set; }

        /// <summary>
        /// Search for products created before this date.
        /// </summary>
        public DateTime? CreatedTo { get; set; }

        /// <summary>
        /// Search for products updated after this date.
        /// </summary>
        public DateTime? UpdatedFrom { get; set; }

        /// <summary>
        /// Search for product updated before this date.
        /// </summary>
        public DateTime? UpdatedTo { get; set; }

        /// <summary>
        /// Sort order.
        /// </summary>
        public SortCriteria SortBy { get; set; } = SortCriteria.NAME_ASC;

        /// <summary>
        /// Number of items to skip at the beginning.
        /// </summary>
        public int Offset { get; set; } = 0;

        /// <summary>
        /// Maximum number of items to return.
        /// </summary>
        public int Limit { get; set; } = 100;
    }

    /// <summary>
    /// Sorting criteria.
    /// </summary>
    public enum SortCriteria
    {
        /// <summary>
        /// Sort products in ascending order by name.
        /// </summary>
        NAME_ASC,

        /// <summary>
        /// Sort products in descending order by name.
        /// </summary>
        NAME_DESC,

        /// <summary>
        /// Sort products in ascending order by price.
        /// </summary>
        PRICE_ASC,

        /// <summary>
        /// Sort products in descending order by price.
        /// </summary>
        PRICE_DESC,

        /// <summary>
        /// Sort products in ascending order by date of creation.
        /// </summary>
        CREATED_ASC,

        /// <summary>
        /// Sort products in descending order by date of creation.
        /// </summary>
        CREATED_DESC,

        /// <summary>
        /// Sort products in ascending order by date of last modification.
        /// </summary>
        UPDATED_ASC,

        /// <summary>
        /// Sort products in descending order by date of last modification.
        /// </summary>
        UPDATED_DESC
    }
}
