using Ecommerce.Common.Interfaces;
using System.Text.Json.Serialization;

namespace Ecommerce.Common.Models.Schema
{
    /// <summary>
    /// Media image model.
    /// </summary>
    public class MediaImage : IEntity
    {
        /// <summary>
        /// Image ID.
        /// </summary>
        /// <example>10372</example>
        public int Id { get; set; }

        /// <summary>
        /// Image URL.
        /// </summary>
        public string Url { get; set; } = string.Empty;

        /// <summary>
        /// Filename.
        /// </summary>
        /// <example>df798984-3634-4c81-8eb0-19604d313302.png</example>
        public string Filename { get; set; } = string.Empty;

        /// <summary>
        /// Mime type.
        /// </summary>
        /// <example>image/png</example>
        public string MimeType { get; set; } = string.Empty;

        /// <summary>
        /// Image width in pixels.
        /// </summary>
        /// <example>640</example>
        public int Width { get; set; }

        /// <summary>
        /// Image height in pixels.
        /// </summary>
        /// <example>640</example>
        public int Height { get; set; }

        /// <summary>
        /// Datetime of creation.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Datetime of last modification.
        /// </summary>
        [JsonIgnore]
        public DateTime UpdatedAt { get; set; }
    }
}
