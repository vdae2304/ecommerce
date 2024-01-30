using Ecommerce.Common.Models.Responses;
using Ecommerce.Controllers.IAM.Password;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers.IAM.Email
{
    [Route("api/iam/email")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<EmailController> _logger;

        public EmailController(IMediator mediator, ILogger<EmailController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// Create a code for email confirmation.
        /// </summary>
        /// <param name="request">User credentials.</param>
        /// <response code="200">Ok. Send confirmation code by email.</response>
        /// <response code="400">Bad request. Invalid field.</response>
        [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [Consumes("application/x-www-form-urlencoded")]
        [Produces("application/json")]
        [HttpPost("send")]
        public async Task<IActionResult> Send([FromForm] SendConfirmationRequest request)
        {
            _logger.LogInformation("Send email confirmation code to {email}", request.Email);
            return await _mediator.Send(request);
        }

        /// <summary>
        /// Validate an email confirmation code.
        /// </summary>
        /// <param name="request">User credentials.</param>
        /// <response code="200">Ok. Confirm email.</response>
        /// <response code="400">Bad request. Invalid field.</response>
        [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [Consumes("application/x-www-form-urlencoded")]
        [Produces("application/json")]
        [HttpPost("confirm")]
        public async Task<IActionResult> Confirm([FromForm] ConfirmEmailRequest request)
        {
            _logger.LogInformation("Verify email {email}", request.Email);
            return await _mediator.Send(request);
        }
    }
}
