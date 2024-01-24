using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace Ecommerce.Controllers.Payment.EditPaymentMethod
{
    public record EditPaymentMethodRequest : IRequest<IActionResult>
    {
        /// <summary>
        /// User ID linked to the payment method.
        /// </summary>
        [JsonIgnore]
        public int UserId { get; set; }

        /// <summary>
        /// Payment method ID.
        /// </summary>
        [JsonIgnore]
        public int PaymentMethodId { get; set; }

        /// <summary>
        /// Card owner.
        /// </summary>
        public string? CardOwner { get; set; }

        /// <summary>
        /// Card verification value.
        /// </summary>
        public string? CVV { get; set; }

        /// <summary>
        /// Month of expiration.
        /// </summary>
        public int? ExpiryMonth { get; set; }

        /// <summary>
        /// Year of expiration.
        /// </summary>
        public int? ExpiryYear { get; set; }

        /// <summary>
        /// Billing address ID.
        /// </summary>
        public int? BillingAddressId { get; set; }
    }
}
