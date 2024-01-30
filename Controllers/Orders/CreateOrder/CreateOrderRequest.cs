using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace Ecommerce.Controllers.Orders.CreateOrder
{
    public record CreateOrderRequest : IRequest<IActionResult>
    {
        /// <summary>
        /// User ID who placed the order.
        /// </summary>
        [JsonIgnore]
        public int UserId { get; set; }

        /// <summary>
        /// List of products contained in the order.
        /// </summary>
        [JsonRequired]
        public IEnumerable<CreateOrderProductRequest> Items { get; set; } = new List<CreateOrderProductRequest>();

        /// <summary>
        /// Payment method ID.
        /// </summary>
        [JsonRequired]
        public int PaymentMethodId { get; set; }

        /// <summary>
        /// Shipping address ID.
        /// </summary>
        [JsonRequired]
        public int ShippingAddressId { get; set; }
    }

    public record CreateOrderProductRequest
    {
        /// <summary>
        /// Product ID.
        /// </summary>
        [JsonRequired]
        public int ProductId { get; set; }
        /// <summary>
        /// Product quantity.
        /// </summary>
        [JsonRequired]
        public int Quantity { get; set; }
    }
}
