namespace Ecommerce.Common.Models.Responses
{
    /// <summary>
    /// Searching results.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SearchItems<T> where T : class
    {
        /// <summary>
        /// Number of items matching the query.
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// Number of items returned.
        /// </summary>
        public int Count => Items.Count();
        
        /// <summary>
        /// Number of items to skip at the beginning.
        /// </summary>
        public int Offset { get; set; }
        
        /// <summary>
        /// Maximum number of items to return.
        /// </summary>
        public int Limit { get; set; }
        
        /// <summary>
        /// List of items returned.
        /// </summary>
        public IEnumerable<T> Items { get; set; } = new List<T>();
    }
}
