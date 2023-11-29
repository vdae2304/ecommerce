using Ecommerce.Common.Interfaces;
using System.Text.Json.Serialization;

namespace Ecommerce.Common.Models.Schema
{
    /// <summary>
    /// Media image.
    /// </summary>
    public class MediaImage : IEntity
    {
        /// <summary>
        /// Image ID.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// File ID.
        /// </summary>
        [JsonIgnore]
        public string FileId { get; set; } = string.Empty;

        /// <summary>
        /// Image URL.
        /// </summary>
        public string Url { get; set; } = string.Empty;

        /// <summary>
        /// Image width in pixels.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Image height in pixels.
        /// </summary>
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
