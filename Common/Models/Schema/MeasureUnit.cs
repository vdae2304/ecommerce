using Ecommerce.Common.Interfaces;
using System.Text.Json.Serialization;

namespace Ecommerce.Common.Models.Schema
{
    /// <summary>
    /// Measure unit.
    /// </summary>
    public class MeasureUnit : IEntity
    {
        /// <summary>
        /// Unit ID.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Unit symbol.
        /// </summary>
        public string Symbol { get; set; } = string.Empty;

        /// <summary>
        /// Unit name.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Unit type.
        /// </summary>
        public MeasureUnitType Type { get; set; }

        /// <summary>
        /// Datetime of creation.
        /// </summary>
        [JsonIgnore]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Datetime of last modification.
        /// </summary>
        [JsonIgnore]
        public DateTime UpdatedAt { get; set; }
    }

    /// <summary>
    /// Measure unit types.
    /// </summary>
    public enum MeasureUnitType
    {
        /// <summary>
        /// Unit of measure for dimension.
        /// </summary>
        Dimension = 0,
        /// <summary>
        /// Unit of measure for weight.
        /// </summary>
        Weight = 1,
        /// <summary>
        /// Unit of measure for volume.
        /// </summary>
        Volume = 2
    }
}
