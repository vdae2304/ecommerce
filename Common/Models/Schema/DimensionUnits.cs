namespace Ecommerce.Common.Models.Schema
{
    /// <summary>
    /// Units of measurement for dimension.
    /// </summary>
    public enum DimensionUnits
    {
        /// <summary>
        /// Centimeters.
        /// </summary>
        Centimeters = 0,
        /// <summary>
        /// Millimeters.
        /// </summary>
        Millimeters = 1,
        /// <summary>
        /// Meters.
        /// </summary>
        Meters = 2,
        /// <summary>
        /// Inches.
        /// </summary>
        Inches = 3,
        /// <summary>
        /// Feet.
        /// </summary>
        Feet = 4,
        /// <summary>
        /// Yards.
        /// </summary>
        Yards = 5
    }

    public static class DimensionUnitsSymbols
    {
        public static string Symbol(this DimensionUnits dimensionUnits)
        {
            return dimensionUnits switch
            {
                DimensionUnits.Centimeters => "cm",
                DimensionUnits.Millimeters => "mm",
                DimensionUnits.Meters => "m",
                DimensionUnits.Inches => "in",
                DimensionUnits.Feet => "ft",
                DimensionUnits.Yards => "yd",
                _ => string.Empty,
            };
        }
    }
}
