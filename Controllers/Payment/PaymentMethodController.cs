using Ecommerce.Common.Exceptions;
using Ecommerce.Common.Models.Orders;
using Ecommerce.Common.Models.Responses;
using Ecommerce.Controllers.IAM;
using Ecommerce.Controllers.Payment.CreatePaymentMethod;
using Ecommerce.Controllers.Payment.DeletePaymentMethod;
using Ecommerce.Controllers.Payment.EditPaymentMethod;
using Ecommerce.Controllers.Payment.GetPaymentMethod;
using Ecommerce.Controllers.Payment.SearchPaymentMethods;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers.Payment
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/payment")]
    [ApiController]
    public class PaymentMethodController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<PaymentMethodController> _logger;

        public PaymentMethodController(IMediator mediator, ILogger<PaymentMethodController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// Get all payment methods for an user.
        /// </summary>
        /// <param name="filters">Search filters.</param>
        /// <response code="200">Ok. Return the list payment methods.</response>
        /// <response code="401">Unauthorized. User is not logged in.</response>
        [ProducesResponseType(typeof(Response<SearchItems<PaymentMethod>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] PaymentMethodFilters filters)
        {
            filters.UserId = User.GetUserId() ?? throw new UnauthorizedException("");
            _logger.LogInformation("Get payment methods for user {userId}", filters.UserId);
            return await _mediator.Send(filters);
        }

        /// <summary>
        /// Get details for a payment method.
        /// </summary>
        /// <param name="paymentMethodId">Payment method ID.</param>
        /// <response code="200">Ok. Return the payment method details.</response>
        /// <response code="401">Unauthorized. User is not logged in.</response>
        /// <response code="404">Not Found. Payment method does not exist.</response>
        [ProducesResponseType(typeof(Response<PaymentMethod>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
        [HttpGet("{paymentMethodId}")]
        public async Task<IActionResult> Get(int paymentMethodId)
        {
            int userId = User.GetUserId() ?? throw new UnauthorizedException("");
            _logger.LogInformation("Get details for payment method {paymentMethodId}", paymentMethodId);
            return await _mediator.Send(new GetPaymentMethodRequest { UserId = userId, PaymentMethodId = paymentMethodId });
        }

        /// <summary>
        /// Create a payment method.
        /// </summary>
        /// <param name="request">Payment method values.</param>
        /// <response code="200">Ok. Return the ID of the payment method created.</response>
        /// <response code="400">Bad request. Invalid field.</response>
        /// <response code="401">Unauthorized. User is not logged in.</response>
        [ProducesResponseType(typeof(Response<CreatedId>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePaymentMethodForm request)
        {
            request.UserId = User.GetUserId() ?? throw new UnauthorizedException("");
            _logger.LogInformation("Create payment method");
            return await _mediator.Send(request);
        }

        /// <summary>
        /// Edit a payment method.
        /// </summary>
        /// <param name="paymentMethodId">Payment method ID.</param>
        /// <param name="request">Payment method values</param>
        /// <response code="200">Ok. Update the payment method.</response>
        /// <response code="400">Bad request. Invalid field.</response>
        /// <response code="401">Unauthorized. User is not logged in.</response>
        /// <response code="404">Not Found. Payment method does not exist.</response>
        [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
        [HttpPatch("{paymentMethodId}")]
        public async Task<IActionResult> Edit(int paymentMethodId, [FromBody] EditPaymentMethodRequest request)
        {
            request.UserId = User.GetUserId() ?? throw new UnauthorizedException("");
            request.PaymentMethodId = paymentMethodId;
            _logger.LogInformation("Edit payment method {paymentMethodId}", paymentMethodId);
            return await _mediator.Send(request);
        }

        /// <summary>
        /// Delete a payment method.
        /// </summary>
        /// <param name="paymentMethodId">Payment method ID.</param>
        /// <response code="200">Ok. Delete the payment method.</response>
        /// <response code="401">Unauthorized. User is not logged in.</response>
        /// <response code="404">Not Found. Payment method does not exist.</response>
        [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
        [HttpDelete("{paymentMethodId}")]
        public async Task<IActionResult> Delete(int paymentMethodId)
        {
            int userId = User.GetUserId() ?? throw new UnauthorizedException("");
            _logger.LogInformation("Delete payment method {paymentMethodId}", paymentMethodId);
            return await _mediator.Send(new DeletePaymentMethodRequest { UserId = userId, PaymentMethodId = paymentMethodId });
        }
    }
}
