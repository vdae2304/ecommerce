using Ecommerce.Common.Exceptions;
using Ecommerce.Common.Models.Orders;
using Ecommerce.Common.Models.Responses;
using Ecommerce.Controllers.IAM;
using Ecommerce.Controllers.Shipping.CreateAddress;
using Ecommerce.Controllers.Shipping.DeleteAddress;
using Ecommerce.Controllers.Shipping.EditAddress;
using Ecommerce.Controllers.Shipping.GetAddress;
using Ecommerce.Controllers.Shipping.SearchAddresses;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers.Shipping
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/shipping")]
    [ApiController]
    public class ShippingAddressController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ShippingAddressController> _logger;

        public ShippingAddressController(IMediator mediator, ILogger<ShippingAddressController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// Get all shipping addresses for an user.
        /// </summary>
        /// <param name="filters">Search filters.</param>
        /// <response code="200">Ok. Return the shipping address.</response>
        /// <response code="401">Unauthorized. User is not logged in.</response>
        [ProducesResponseType(typeof(Response<SearchItems<ShippingAddress>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
        [HttpGet()]
        public async Task<IActionResult> Search([FromQuery] AddressFilters filters)
        {
            filters.UserId = User.GetUserId() ?? throw new UnauthorizedException("");
            _logger.LogInformation("Get shipping addresses for user {userId}", filters.UserId);
            return await _mediator.Send(filters);
        }

        /// <summary>
        /// Get details for a shipping address.
        /// </summary>
        /// <param name="shippingAddressId">Shipping address ID.</param>
        /// <response code="200">Ok. Return the shipping address.</response>
        /// <response code="401">Unauthorized. User is not logged in.</response>
        /// <response code="404">Not Found. Shipping address does not exist.</response>
        [ProducesResponseType(typeof(Response<ShippingAddress>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
        [HttpGet("{shippingAddressId}")]
        public async Task<IActionResult> Get(int shippingAddressId)
        {
            int userId = User.GetUserId() ?? throw new UnauthorizedException("");
            _logger.LogInformation("Get details for shipping address {shippingAddressId}", shippingAddressId);
            return await _mediator.Send(new GetAddressRequest { UserId = userId, ShippingAddressId = shippingAddressId });
        }

        /// <summary>
        /// Create a shipping address.
        /// </summary>
        /// <param name="request">Shipping address values.</param>
        /// <response code="200">Ok. Return the ID of the shipping address created.</response>
        /// <response code="400">Bad request. Invalid field.</response>
        /// <response code="401">Unauthorized. User is not logged in.</response>
        [ProducesResponseType(typeof(Response<CreatedId>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAddressForm request)
        {
            request.UserId = User.GetUserId() ?? throw new UnauthorizedException("");
            _logger.LogInformation("Create shipping address");
            return await _mediator.Send(request);
        }

        /// <summary>
        /// Edit a shipping address.
        /// </summary>
        /// <param name="shippingAddressId">Shipping address ID.</param>
        /// <param name="request">Shipping address values.</param>
        /// <response code="200">Ok. Update the shipping address.</response>
        /// <response code="400">Bad request. Invalid field.</response>
        /// <response code="401">Unauthorized. User is not logged in.</response>
        /// <response code="404">Not Found. Shipping address does not exist.</response>
        [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
        [HttpPut("{shippingAddressId}")]
        public async Task<IActionResult> Edit(int shippingAddressId, EditAddressRequest request)
        {
            request.UserId = User.GetUserId() ?? throw new UnauthorizedException("");
            request.ShippingAddressId = shippingAddressId;
            _logger.LogInformation("Edit shipping address {shippingAddressId}", shippingAddressId);
            return await _mediator.Send(request);
        }

        /// <summary>
        /// Delete a shipping address.
        /// </summary>
        /// <param name="shippingAddressId">Shipping address ID.</param>
        /// <response code="200">Ok. Delete the shipping address.</response>
        /// <response code="401">Unauthorized. User is not logged in.</response>
        /// <response code="404">Not Found. Shipping address does not exist.</response>
        [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
        [HttpDelete("{shippingAddressId}")]
        public async Task<IActionResult> Delete(int shippingAddressId)
        {
            int userId = User.GetUserId() ?? throw new UnauthorizedException("");
            _logger.LogInformation("Delete shipping address {shippingAddressId}", shippingAddressId);
            return await _mediator.Send(new DeleteAddressRequest { UserId = userId, ShippingAddressId = shippingAddressId });
        }
    }
}
