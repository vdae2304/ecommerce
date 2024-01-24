using Ecommerce.Common.Models.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers.IAM.Login
{
    [Route("api/iam/login")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<LoginController> _logger;

        public LoginController(IMediator mediator, ILogger<LoginController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// Create a user session.
        /// </summary>
        /// <param name="request">User credentials.</param>
        /// <response code="200">Ok. Return a token.</response>
        /// <response code="401">Unauthorized. Invalid credentials.</response>
        [ProducesResponseType(typeof(Response<Authentication>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [Consumes("application/x-www-form-urlencoded")]
        [Produces("application/json")]
        [HttpPost]
        public async Task<IActionResult> Login([FromForm] LoginRequest request)
        {
            _logger.LogInformation("Login user {userName}", request.UserName);
            return await _mediator.Send(request);
        }
    }
}
