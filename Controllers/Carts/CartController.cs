using Ecommerce.Common.Exceptions;
using Ecommerce.Common.Models.Orders;
using Ecommerce.Common.Models.Responses;
using Ecommerce.Controllers.Carts.AddProduct;
using Ecommerce.Controllers.Carts.GetCart;
using Ecommerce.Controllers.Carts.RemoveProduct;
using Ecommerce.Controllers.IAM;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers.Carts
{
    [Route("api/cart")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CartController> _logger;

        public CartController(IMediator mediator, ILogger<CartController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// Get products in the cart.
        /// </summary>
        /// <response code="200">Ok. Return the product in the cart.</response>
        /// <response code="401">Unauthorized. User is not logged in.</response>
        [ProducesResponseType(typeof(Response<List<Cart>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            int userId = User.GetUserId() ?? throw new UnauthorizedException("");
            _logger.LogInformation("Get cart products for user {userId}", userId);
            return await _mediator.Send(new GetCartRequest { UserId = userId });
        }

        /// <summary>
        /// Add a product to the cart.
        /// </summary>
        /// <param name="request">Product values.</param>
        /// <response code="200">Ok. Add the product.</response>
        /// <response code="401">Unauthorized. User is not logged in.</response>
        [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPost()]
        public async Task<IActionResult> AddProduct([FromBody] AddProductRequest request)
        {
            request.UserId = User.GetUserId() ?? throw new UnauthorizedException("");
            _logger.LogInformation("Add product {productId} to cart", request.ProductId);
            return await _mediator.Send(request);
        }

        /// <summary>
        /// Delete a product from the cart.
        /// </summary>
        /// <param name="productId">Product ID.</param>
        /// <response code="200">Ok. Delete the product.</response>
        /// <response code="401">Unauthorized. User is not logged in.</response>
        [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        [HttpDelete("{productId}")]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            int userId = User.GetUserId() ?? throw new UnauthorizedException("");
            _logger.LogInformation("Delete product {productId} from cart", productId);
            return await _mediator.Send(new RemoveProductRequest { UserId = userId, ProductId = productId });
        }
    }
}
