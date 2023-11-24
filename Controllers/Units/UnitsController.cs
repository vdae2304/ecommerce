using Ecommerce.Common.Models.Responses;
using Ecommerce.Common.Models.Schema;
using Ecommerce.Controllers.Units.CreateUnit;
using Ecommerce.Controllers.Units.DeleteUnit;
using Ecommerce.Controllers.Units.GetUnits;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers.Units
{
    [Route("api/units")]
    [ApiController]
    public class UnitsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<UnitsController> _logger;

        public UnitsController(IMediator mediator, ILogger<UnitsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// Get all units of measurement within a group.
        /// </summary>
        /// <param name="unitType">Unit type</param>
        /// <response code="200">Ok. Create the unit.</response>
        /// <response code="404">Not Found. Invalid unit type.</response>
        [ProducesResponseType(typeof(DataResponse<List<MeasureUnit>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(StatusResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(StatusResponse), StatusCodes.Status500InternalServerError)]
        [HttpGet("{unitType}")]
        public async Task<ActionResult<DataResponse<List<MeasureUnit>>>> Get(MeasureUnitType unitType)
        {
            _logger.LogInformation("Get units of measurement for {type}", unitType);
            return await _mediator.Send(new GetUnitsRequest { UnitType = unitType });
        }

        /// <summary>
        /// Add a new unit of measurement.
        /// </summary>
        /// <param name="request">Unit values.</param>
        /// <response code="200">Ok. Create the unit.</response>
        /// <response code="400">Bad request. Invalid field.</response>
        [ProducesResponseType(typeof(StatusResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(StatusResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(StatusResponse), StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public async Task<ActionResult<StatusResponse>> Create([FromForm] CreateUnitForm request)
        {
            _logger.LogInformation("Create unit of measurement {symbol}", request.Symbol);
            return await _mediator.Send(request);
        }

        /// <summary>
        /// Delete an unit of measurement.
        /// </summary>
        /// <param name="unitId">Unit ID.</param>
        /// <response code="200">Ok. Delete the unit.</response>
        /// <response code="404">Not Found. Unit does not exist.</response>
        [ProducesResponseType(typeof(StatusResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(StatusResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(StatusResponse), StatusCodes.Status500InternalServerError)]
        [HttpDelete("{unitId}")]
        public async Task<ActionResult<StatusResponse>> Delete(int unitId)
        {
            _logger.LogInformation("Delete unit of measurement {id}", unitId);
            return await _mediator.Send(new DeleteUnitRequest { UnitId = unitId });
        }
    }
}
