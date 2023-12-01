namespace Ecommerce.Common.Models.Schema
{
    /// <summary>
    /// Units of measurement for weight.
    /// </summary>
    public enum WeightUnits
    {
        /// <summary>
        /// Kilogram.
        /// </summary>
        Kilogram = 0,
        /// <summary>
        /// Gram.
        /// </summary>
        Gram = 1,
        /// <summary>
        /// Ounce.
        /// </summary>
        Ounce = 3,
        /// <summary>
        /// Pound.
        /// </summary>
        Pound = 4
    }

    public static partial class WeightUnitsSymbols
    {
        public static string Symbol(this WeightUnits weightUnits)
        {
            return weightUnits switch
            {
                WeightUnits.Kilogram => "kg",
                WeightUnits.Gram => "g",
                WeightUnits.Ounce => "oz",
                WeightUnits.Pound => "lb",
                _ => string.Empty,
            };
        }
    }
}
