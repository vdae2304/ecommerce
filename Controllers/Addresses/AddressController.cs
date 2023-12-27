using Ecommerce.Common.Exceptions;
using Ecommerce.Common.Models.Orders;
using Ecommerce.Common.Models.Responses;
using Ecommerce.Controllers.IAM;
using Ecommerce.Controllers.Addresses.CreateAddress;
using Ecommerce.Controllers.Addresses.DeleteAddress;
using Ecommerce.Controllers.Addresses.EditAddress;
using Ecommerce.Controllers.Addresses.GetAddress;
using Ecommerce.Controllers.Addresses.SearchAddresses;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers.Addresses
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/address")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AddressController> _logger;

        public AddressController(IMediator mediator, ILogger<AddressController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// Get all addresses for an user.
        /// </summary>
        /// <param name="filters">Search filters.</param>
        /// <response code="200">Ok. Return the list of addresses.</response>
        /// <response code="401">Unauthorized. User is not logged in.</response>
        [ProducesResponseType(typeof(Response<SearchItems<Address>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        [HttpGet()]
        public async Task<IActionResult> Search([FromQuery] AddressFilters filters)
        {
            filters.UserId = User.GetUserId() ?? throw new UnauthorizedException("");
            _logger.LogInformation("Get addresses for user {userId}", filters.UserId);
            return await _mediator.Send(filters);
        }

        /// <summary>
        /// Get details for an address.
        /// </summary>
        /// <param name="addressId">Address ID.</param>
        /// <response code="200">Ok. Return the address.</response>
        /// <response code="401">Unauthorized. User is not logged in.</response>
        /// <response code="404">Not Found. Address does not exist.</response>
        [ProducesResponseType(typeof(Response<Address>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        [HttpGet("{addressId}")]
        public async Task<IActionResult> Get(int addressId)
        {
            int userId = User.GetUserId() ?? throw new UnauthorizedException("");
            _logger.LogInformation("Get details for address {addressId}", addressId);
            return await _mediator.Send(new GetAddressRequest { UserId = userId, AddressId = addressId });
        }

        /// <summary>
        /// Create an address.
        /// </summary>
        /// <param name="request">Address values.</param>
        /// <response code="200">Ok. Return the ID of the address created.</response>
        /// <response code="400">Bad request. Invalid field.</response>
        /// <response code="401">Unauthorized. User is not logged in.</response>
        [ProducesResponseType(typeof(Response<CreatedId>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAddressForm request)
        {
            request.UserId = User.GetUserId() ?? throw new UnauthorizedException("");
            _logger.LogInformation("Create address");
            return await _mediator.Send(request);
        }

        /// <summary>
        /// Edit an address.
        /// </summary>
        /// <param name="addressId">Address ID.</param>
        /// <param name="request">Address values.</param>
        /// <response code="200">Ok. Update the address.</response>
        /// <response code="400">Bad request. Invalid field.</response>
        /// <response code="401">Unauthorized. User is not logged in.</response>
        /// <response code="404">Not Found. Address does not exist.</response>
        [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPut("{addressId}")]
        public async Task<IActionResult> Edit(int addressId, EditAddressRequest request)
        {
            request.UserId = User.GetUserId() ?? throw new UnauthorizedException("");
            request.AddressId = addressId;
            _logger.LogInformation("Edit address {addressId}", addressId);
            return await _mediator.Send(request);
        }

        /// <summary>
        /// Delete an address.
        /// </summary>
        /// <param name="addressId">Address ID.</param>
        /// <response code="200">Ok. Delete the address.</response>
        /// <response code="401">Unauthorized. User is not logged in.</response>
        /// <response code="404">Not Found. Address does not exist.</response>
        [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        [HttpDelete("{addressId}")]
        public async Task<IActionResult> Delete(int addressId)
        {
            int userId = User.GetUserId() ?? throw new UnauthorizedException("");
            _logger.LogInformation("Delete address {addressId}", addressId);
            return await _mediator.Send(new DeleteAddressRequest { UserId = userId, AddressId = addressId });
        }
    }
}
