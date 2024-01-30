using Ecommerce.Common.Models.Orders;
using Ecommerce.Common.Models.Responses;
using Ecommerce.Controllers.IAM;
using Ecommerce.Controllers.Orders.CreateOrder;
using Ecommerce.Controllers.Orders.GetOrder;
using Ecommerce.Controllers.Orders.SearchOrders;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers.Orders
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(IMediator mediator, ILogger<OrdersController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// Get all orders created by an user.
        /// </summary>
        /// <param name="filters">Search filters.</param>
        /// <response code="200">Ok. Return the list of orders.</response>
        /// <response code="401">Unauthorized. User is not logged in.</response>
        [ProducesResponseType(typeof(Response<SearchItems<Order>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] SearchOrdersRequest filters)
        {
            filters.UserId = User.GetUserId()!.Value;
            _logger.LogInformation("Search orders for user {userId}", filters.UserId);
            return await _mediator.Send(filters);
        }

        /// <summary>
        /// Place an order.
        /// </summary>
        /// <param name="request">Order items.</param>
        /// <response code="200">Ok. Return the ID of the order created.</response>
        /// <response code="400">Bad Request. Invalid field.</response>
        /// <response code="401">Unauthorized. User is not logged in.</response>
        [ProducesResponseType(typeof(Response<CreatedId>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateOrderRequest request)
        {
            request.UserId = User.GetUserId()!.Value;
            _logger.LogInformation("Create order for user {userId}", request.UserId);
            return await _mediator.Send(request);
        }

        /// <summary>
        /// Get details for an order.
        /// </summary>
        /// <param name="orderId">Order ID.</param>
        /// <response code="200">Ok. Return the order.</response>
        /// <response code="401">Unauthorized. User is not logged in.</response>
        /// <response code="404">Not Found. Order does not exist.</response>
        [ProducesResponseType(typeof(Response<Order>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        [HttpGet("{orderId}")]
        public async Task<IActionResult> Get(int orderId)
        {
            int userId = User.GetUserId()!.Value;
            _logger.LogInformation("Get details for order {orderId}", orderId);
            return await _mediator.Send(new GetOrderRequest { UserId = userId, OrderId = orderId });
        }
    }
}
