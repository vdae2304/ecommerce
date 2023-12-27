using Ecommerce.Common.Interfaces;
using System.Text.Json.Serialization;

namespace Ecommerce.Common.Models.Orders
{
    /// <summary>
    /// Address model.
    /// </summary>
    public class Address : IEntity
    {
        /// <summary>
        /// Address ID.
        /// </summary>
        /// <example>71928</example>
        public int Id { get; set; }

        /// <summary>
        /// User ID linked to the address.
        /// </summary>
        [JsonIgnore]
        public int UserId { get; set; }

        /// <summary>
        /// Recipient name.
        /// </summary>
        /// <example>John Doe</example>
        public string Recipient { get; set; } = string.Empty;

        /// <summary>
        /// Contact phone number.
        /// </summary>
        /// <example>5523917391</example>
        public string Phone { get; set; } = string.Empty;

        /// <summary>
        /// Street name.
        /// </summary>
        /// <example>Av. Francisco I. Madero</example>
        public string Street { get; set; } = string.Empty;

        /// <summary>
        /// Street number.
        /// </summary>
        /// <example>17</example>
        public string StreetNumber { get; set; } = string.Empty;

        /// <summary>
        /// Apartment number, if any.
        /// </summary>
        /// <example>null</example>
        public string? AptNumber { get; set; }

        /// <summary>
        /// Neighbourhood.
        /// </summary>
        /// <example>Centro</example>
        public string Neighbourhood { get; set; } = string.Empty;

        /// <summary>
        /// City.
        /// </summary>
        /// <example>Mexico City</example>
        public string City { get; set; } = string.Empty;

        /// <summary>
        /// State/province code.
        /// </summary>
        /// <example>CDMX</example>
        public string State { get; set; } = string.Empty;

        /// <summary>
        /// Postal code (zip code).
        /// </summary>
        /// <example>06050</example>
        public string PostalCode { get; set; } = string.Empty;

        /// <summary>
        /// Additional comments.
        /// </summary>
        public string? Comments { get; set; }

        /// <summary>
        /// Datetime of creation.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Datetime of last modification.
        /// </summary>
        public DateTime UpdatedAt { get; set; }
    }
}
