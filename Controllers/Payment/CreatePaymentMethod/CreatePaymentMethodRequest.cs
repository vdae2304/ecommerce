using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace Ecommerce.Controllers.Payment.CreatePaymentMethod
{
    public record CreatePaymentMethodRequest : IRequest<IActionResult>
    {
        /// <summary>
        /// User ID linked to the payment method.
        /// </summary>
        [JsonIgnore]
        public int UserId { get; set; }

        /// <summary>
        /// Card owner.
        /// </summary>
        [JsonRequired]
        public string CardOwner { get; set; } = string.Empty;

        /// <summary>
        /// Card number.
        /// </summary>
        [JsonRequired]
        public string CardNumber { get; set; } = string.Empty;

        /// <summary>
        /// Card verification value.
        /// </summary>
        [JsonRequired]
        public string CVV { get; set; } = string.Empty;

        /// <summary>
        /// Month of expiration.
        /// </summary>
        [JsonRequired]
        public int ExpiryMonth { get; set; }

        /// <summary>
        /// Year of expiration.
        /// </summary>
        [JsonRequired]
        public int ExpiryYear { get; set; }

        /// <summary>
        /// Billing address ID.
        /// </summary>
        [JsonRequired]
        public int BillingAddressId { get; set; }
    }
}
