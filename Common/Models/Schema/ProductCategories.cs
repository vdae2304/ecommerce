namespace Ecommerce.Common.Models.Schema
{
    /// <summary>
    /// Relationship between products and categories.
    /// </summary>
    public class ProductCategories
    {
        /// <summary>
        /// Category ID.
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// Product ID.
        /// </summary>
        public int ProductId { get; set; }
    }
}
