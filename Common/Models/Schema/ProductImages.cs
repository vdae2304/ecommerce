namespace Ecommerce.Common.Models.Schema
{
    /// <summary>
    /// Relationship between products and images.
    /// </summary>
    public class ProductImages
    {
        /// <summary>
        /// Product ID.
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Image ID.
        /// </summary>
        public int ImageId { get; set; }
    }
}
