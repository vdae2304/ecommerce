using Ecommerce.Common.Interfaces;
using System.Text.Json.Serialization;

namespace Ecommerce.Common.Models.Schema
{
    /// <summary>
    /// Product.
    /// </summary>
    public class Product : IEntity
    {
        /// <summary>
        /// Product ID.
        /// </summary>
        /// <remarks>An unique identifier provided by the system.</remarks>
        public int Id { get; set; }
        
        /// <summary>
        /// Product SKU.
        /// </summary>
        /// <remarks>An unique identifier provided by the user.</remarks>
        public string Sku { get; set; } = string.Empty;
        
        /// <summary>
        /// Product name.
        /// </summary>
        public string Name { get; set; } = string.Empty;
        
        /// <summary>
        /// Product description.
        /// </summary>
        public string Description { get; set; } = string.Empty;
        
        /// <summary>
        /// Product price.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Formatted price.
        /// </summary>
        public string FormattedPrice => $"{Price:C}";
        
        /// <summary>
        /// If not null, display a price to compare to.
        /// </summary>
        /// <remarks>When a discount is available, Price displays the price
        /// with discount while CrossedOutPrice displays the original price.</remarks>
        public decimal? CrossedOutPrice { get; set; }

        /// <summary>
        /// Formatted crossed out price.
        /// </summary>
        public string? FormattedCrossedOutPrice => (CrossedOutPrice != null) ? $"{CrossedOutPrice:C}" : null;

        /// <summary>
        /// Thumbnail ID.
        /// </summary>
        [JsonIgnore]
        public int? ThumbnailId { get; set; }
        
        /// <summary>
        /// Thumbnail.
        /// </summary>
        public virtual Image? Thumbnail { get; set; }
        
        /// <summary>
        /// Gallery images.
        /// </summary>
        public virtual ICollection<Image> GalleryImages { get; set; } = new List<Image>();

        /// <summary>
        /// ID of the categories the product is assigned to.
        /// </summary>
        public virtual ICollection<int> CategoryIds => Categories.Select(x => x.Id).ToList();

        /// <summary>
        /// List of categories the product is assigned to.
        /// </summary>
        [JsonIgnore]
        public virtual ICollection<Category> Categories { get; set; } = new List<Category>();

        /// <summary>
        /// Product attributes.
        /// </summary>
        public virtual ICollection<ProductAttribute> Attributes { get; set; } = new List<ProductAttribute>();

        /// <summary>
        /// Product width.
        /// </summary>
        public double? Width { get; set; }

        /// <summary>
        /// Formated width.
        /// </summary>
        public string? FormattedWidth => (Width != null && DimensionUnits != null) ? $"{Width} {DimensionUnits.Symbol}" : null;

        /// <summary>
        /// Product height.
        /// </summary>
        public double? Height { get; set; }

        /// <summary>
        /// Formatted height.
        /// </summary>
        public string? FormattedHeight => (Height != null && DimensionUnits != null) ? $"{Height} {DimensionUnits.Symbol}" : null;

        /// <summary>
        /// Product length.
        /// </summary>
        public double? Length { get; set; }

        /// <summary>
        /// Formatted length.
        /// </summary>
        public string? FormattedLength => (Length != null && DimensionUnits != null) ? $"{Length} {DimensionUnits.Symbol}" : null;

        /// <summary>
        /// Dimension units ID.
        /// </summary>
        [JsonIgnore]
        public int? DimensionUnitsId { get; set; }

        /// <summary>
        /// Dimension units.
        /// </summary>
        [JsonIgnore]
        public virtual MeasureUnit? DimensionUnits { get; set; }
        
        /// <summary>
        /// Product weight.
        /// </summary>
        public double? Weight { get; set; }

        /// <summary>
        /// Formatted weight.
        /// </summary>
        public string? FormattedWeight => (Weight != null && WeightUnits != null) ? $"{Weight} {WeightUnits.Symbol}" : null;

        /// <summary>
        /// Weight units ID.
        /// </summary>
        [JsonIgnore]
        public int? WeightUnitsId { get; set; }

        /// <summary>
        /// Weight units.
        /// </summary>
        [JsonIgnore]
        public virtual MeasureUnit? WeightUnits { get; set; }
        
        /// <summary>
        /// Product volume.
        /// </summary>
        public double? Volume { get; set; }

        /// <summary>
        /// Formatted volume.
        /// </summary>
        public string? FormattedVolume => (Volume != null && VolumeUnits != null) ? $"{Volume} {VolumeUnits.Symbol}" : null;

        /// <summary>
        /// Volume units ID.
        /// </summary>
        [JsonIgnore]
        public int? VolumeUnitsId { get; set; }

        /// <summary>
        /// Volume units.
        /// </summary>
        [JsonIgnore]
        public virtual MeasureUnit? VolumeUnits { get; set; }

        /// <summary>
        /// Minimum allowed purchase quantity.
        /// </summary>
        public int? MinPurchaseQuantity { get; set; }

        /// <summary>
        /// Maximum allowed purchase quantity.
        /// </summary>
        public int? MaxPurchaseQuantity { get; set; }
        
        /// <summary>
        /// Available number of products in stock, or null if unlimited.
        /// </summary>
        public int? InStock { get; set; }

        /// <summary>
        /// Whether the product is enabled or not.
        /// </summary>
        public bool? Enabled { get; set; }

        /// <summary>
        /// Datetime of creation.
        /// </summary>
        public DateTime CreatedAt { get; set; }
        
        /// <summary>
        /// Datetime of last modification.
        /// </summary>
        public DateTime UpdatedAt { get; set; }
    }
}
