using Ecommerce.Common.Exceptions;
using Ecommerce.Common.Models.Responses;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers.IAM.Password
{
    [Route("api/iam/password")]
    [ApiController]
    public class PasswordController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<PasswordController> _logger;

        public PasswordController(IMediator mediator, ILogger<PasswordController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// Change password for the user in session.
        /// </summary>
        /// <param name="request">User credentials.</param>
        /// <response code="200">Ok. Change password.</response>
        /// <response code="400">Bad request. Invalid field.</response>
        /// <response code="401">Unauthorized. User is not logged in.</response>
        [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
        [Consumes("application/x-www-form-urlencoded")]
        [Produces("application/json")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<IActionResult> Change([FromForm] ChangePasswordRequest request)
        {
            request.UserId = User.GetUserId() ?? throw new UnauthorizedException("");
            _logger.LogInformation("Change password for {userId}", request.UserId);
            return await _mediator.Send(request);
        }

        /// <summary>
        /// Create a request token to set a new password.
        /// </summary>
        /// <param name="request">User credentials.</param>
        /// <response code="200">Ok. Send a password reset token by email.</response>
        /// <response code="400">Bad request. Invalid field.</response>
        [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
        [Consumes("application/x-www-form-urlencoded")]
        [Produces("application/json")]
        [HttpPost("forgot")]
        public async Task<IActionResult> Forgot([FromForm] ForgotPasswordRequest request)
        {
            _logger.LogInformation("Password reset token requested for {email}", request.Email);
            return await _mediator.Send(request);
        }

        /// <summary>
        /// Set a new password for an user.
        /// </summary>
        /// <param name="request">User credentials.</param>
        /// <response code="200">Ok. Change password.</response>
        /// <response code="400">Bad request. Invalid field.</response>
        [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
        [Consumes("application/x-www-form-urlencoded")]
        [Produces("application/json")]
        [HttpPost("reset")]
        public async Task<IActionResult> Reset([FromForm] ResetPasswordRequest request)
        {
            _logger.LogInformation("Change password for {email}", request.Email);
            return await _mediator.Send(request);
        }
    }
}
