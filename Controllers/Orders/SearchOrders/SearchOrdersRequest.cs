using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Ecommerce.Controllers.Orders.SearchOrders
{
    public class SearchOrdersRequest : IRequest<IActionResult>
    {
        /// <summary>
        /// User ID.
        /// </summary>
        [BindNever]
        public int UserId { get; set; }

        /// <summary>
        /// Number of items to skip at the beginning.
        /// </summary>
        public int Offset { get; set; } = 0;

        /// <summary>
        /// Maximum number of items to return.
        /// </summary>
        public int Limit { get; set; } = 100;
    }
}
