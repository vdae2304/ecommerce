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
        /// File ID.
        /// </summary>
        /// <remarks>An unique identifier to track the image internally.</remarks>
        [JsonIgnore]
        public string FileId { get; set; } = string.Empty;

        /// <summary>
        /// Image URL.
        /// </summary>
        public string Url { get; set; } = string.Empty;

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
