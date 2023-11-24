namespace Ecommerce.Common.Models.Responses
{
    public class StatusResponse
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
}
