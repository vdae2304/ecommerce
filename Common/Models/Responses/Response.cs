namespace Ecommerce.Common.Models.Responses
{
    /// <summary>
    /// Response message.
    /// </summary>
    public class Response
    {
        /// <summary>
        /// Whether the response was successful.
        /// </summary>
        public bool Success { get; set; }
        
        /// <summary>
        /// Response message.
        /// </summary>
        public string Message { get; set; } = string.Empty;
    }

    /// <summary>
    /// Response message with data.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Response<T> : Response where T : class
    {
        /// <summary>
        /// Response data.
        /// </summary>
        public T? Data { get; set; }
    }
}
