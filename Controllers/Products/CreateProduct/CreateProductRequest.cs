using Ecommerce.Common.Models.Schema;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace Ecommerce.Controllers.Products.CreateProduct
{
    public record CreateProductRequest : IRequest<IActionResult>
    {
        /// <summary>
        /// An unique identifier for the product.
        /// </summary>
        [JsonRequired]
        public string Sku { get; set; } = string.Empty;

        /// <summary>
        /// Product name.
        /// </summary>
        [JsonRequired]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Product description.
        /// </summary>
        [JsonRequired]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Product price.
        /// </summary>
        [JsonRequired]
        public decimal Price { get; set; }

        /// <summary>
        /// If not null, display a price to compare to.
        /// </summary>
        public decimal? CrossedOutPrice { get; set; }

        /// <summary>
        /// ID of the categories the product is assigned to.
        /// </summary>
        public IEnumerable<int> CategoryIds { get; set; } = new List<int>();

        /// <summary>
        /// Product attributes.
        /// </summary>
        public List<CreateAttributeRequest> Attributes { get; set; } = new List<CreateAttributeRequest>();

        /// <summary>
        /// (Optional) Product length.
        /// </summary>
        public double? Length { get; set; }

        /// <summary>
        /// (Optional) Product width.
        /// </summary>
        public double? Width { get; set; }

        /// <summary>
        /// (Optional) Product height.
        /// </summary>
        public double? Height { get; set; }

        /// <summary>
        /// (Optional) Dimension units.
        /// </summary>
        public DimensionUnits? DimensionUnits { get; set; }

        /// <summary>
        /// (Optional) Product weight.
        /// </summary>
        public double? Weight { get; set; }

        /// <summary>
        /// (Optional) Weight units.
        /// </summary>
        public WeightUnits? WeightUnits { get; set; }

        /// <summary>
        /// Minimum allowed purchase quantity. Default is 1.
        /// </summary>
        public int MinPurchaseQuantity { get; set; } = 1;

        /// <summary>
        /// Maximum allowed purchase quantity. Default is 1.
        /// </summary>
        public int MaxPurchaseQuantity { get; set; } = 1;

        /// <summary>
        /// Available number of products in stock.
        /// Default is 0.
        /// </summary>
        public int InStock { get; set; } = 0;

        /// <summary>
        /// Whether the product has unlimited stock.
        /// Default is false.
        /// </summary>
        public bool Unlimited { get; set; } = false;

        /// <summary>
        /// Whether the product is enabled or not.
        /// Default is true.
        /// </summary>
        public bool Enabled { get; set; } = true;
    }

    public record CreateAttributeRequest : IRequest<IActionResult>
    {
        /// <summary>
        /// Attribute name.
        /// </summary>
        [JsonRequired]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Attribute value.
        /// </summary>
        [JsonRequired]
        public string? Value { get; set; }
    }
}
