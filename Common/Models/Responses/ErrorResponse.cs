using System.Text.Json.Serialization;

namespace Ecommerce.Common.Models.Responses
{
    /// <summary>
    /// Error response.
    /// </summary>
    public class ErrorResponse : Response
    {
        /// <summary>
        /// List of errors.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IEnumerable<string>? Errors { get; set; }
    }
}
