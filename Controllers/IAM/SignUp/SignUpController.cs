using Ecommerce.Common.Models.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers.IAM.SignUp
{
    [Route("api/iam/signup")]
    [ApiController]
    public class SignUpController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<SignUpController> _logger;

        public SignUpController(IMediator mediator, ILogger<SignUpController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// Register a new user.
        /// </summary>
        /// <param name="request">User credentials.</param>
        /// <response code="200">Ok. Return the ID of the user created.</response>
        /// <response code="400">Bad request. Invalid field.</response>
        [ProducesResponseType(typeof(Response<UserProfile>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [Consumes("application/x-www-form-urlencoded")]
        [Produces("application/json")]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] SignUpRequest request)
        {
            _logger.LogInformation("Create user {userName}", request.UserName);
            return await _mediator.Send(request);
        }
    }
}
