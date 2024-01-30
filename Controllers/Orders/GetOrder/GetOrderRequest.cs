using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers.Orders.GetOrder
{
    public class GetOrderRequest : IRequest<IActionResult>
    {
        /// <summary>
        /// User ID.
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// Order ID.
        /// </summary>
        public int OrderId { get; set; }
    }
}
