namespace Ecommerce.Common.Models.Responses
{
    public class DataResponse<T> where T : class
    {
        /// <summary>
        /// Whether the response was successful.
        /// </summary>
        public bool Success { get; set; }
        
        /// <summary>
        /// Response message.
        /// </summary>
        public string Message { get; set; } = string.Empty;
        
        /// <summary>
        /// Data.
        /// </summary>
        public T? Data { get; set; }
    }
}
