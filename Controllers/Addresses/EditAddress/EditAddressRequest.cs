using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace Ecommerce.Controllers.Addresses.EditAddress
{
    public record EditAddressRequest : IRequest<IActionResult>
    {
        /// <summary>
        /// User ID linked to the address.
        /// </summary>
        [JsonIgnore]
        public int UserId { get; set; }

        /// <summary>
        /// Address ID.
        /// </summary>
        [JsonIgnore]
        public int AddressId { get; set; }

        /// <summary>
        /// Recipient name.
        /// </summary>
        [JsonRequired]
        public string Recipient { get; set; } = string.Empty;

        /// <summary>
        /// Contact phone number.
        /// </summary>
        [JsonRequired]
        public string Phone { get; set; } = string.Empty;

        /// <summary>
        /// Street name.
        /// </summary>
        [JsonRequired]
        public string Street { get; set; } = string.Empty;

        /// <summary>
        /// Street number.
        /// </summary>
        [JsonRequired]
        public string StreetNumber { get; set; } = string.Empty;

        /// <summary>
        /// Apartment number, if any.
        /// </summary>
        public string? AptNumber { get; set; }

        /// <summary>
        /// Neighbourhood.
        /// </summary>
        [JsonRequired]
        public string Neighbourhood { get; set; } = string.Empty;

        /// <summary>
        /// City.
        /// </summary>
        [JsonRequired]
        public string City { get; set; } = string.Empty;

        /// <summary>
        /// State/province code.
        /// </summary>
        [JsonRequired]
        public string State { get; set; } = string.Empty;

        /// <summary>
        /// Postal code.
        /// </summary>
        [JsonRequired]
        public string PostalCode { get; set; } = string.Empty;

        /// <summary>
        /// Additional comments.
        /// </summary>
        public string? Comments { get; set; }
    }
}
