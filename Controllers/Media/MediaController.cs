using Ecommerce.Common.Models.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers.Media
{
    [Route("api/media")]
    [ApiController]
    public class MediaController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<MediaController> _logger;

        public MediaController(IMediator mediator, ILogger<MediaController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// Get file contents.
        /// </summary>
        /// <param name="filename">Filename.</param>
        /// <response code="200">Ok. Return the file contents.</response>
        /// <response code="404">Not Found. File does not exist.</response>
        /// <returns></returns>
        [ProducesResponseType(typeof(byte[]), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
        [HttpGet("{filename}")]
        public async Task<IActionResult> Get(string filename)
        {
            _logger.LogInformation("Get contents for file {filename}", filename);
            return await _mediator.Send(new GetMediaContentsRequest { Filename = filename });
        }
    }
}
