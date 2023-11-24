using Ecommerce.Common.Interfaces;
using Ecommerce.Common.Models.Responses;
using Ecommerce.Common.Models.Schema;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Controllers.Units.GetUnits
{
    public record GetUnitsRequest : IRequest<ActionResult>
    {
        /// <summary>
        /// Unit type.
        /// </summary>
        public MeasureUnitType UnitType;
    }

    public class GetUnitsHandler : IRequestHandler<GetUnitsRequest, ActionResult>
    {
        private readonly IGenericRepository<MeasureUnit> _units;
        private readonly ILogger<GetUnitsHandler> _logger;

        public GetUnitsHandler(IGenericRepository<MeasureUnit> units, ILogger<GetUnitsHandler> logger)
        {
            _units = units;
            _logger = logger;
        }

        public async Task<ActionResult> Handle(GetUnitsRequest request, CancellationToken cancellationToken)
        {
            try
            {
                List<MeasureUnit> units = await _units.AsQueryable()
                    .Where(x => x.Type == request.UnitType)
                    .ToListAsync(cancellationToken);

                return new OkObjectResult(new DataResponse<List<MeasureUnit>>
                {
                    Success = true,
                    Message = "Ok.",
                    Data = units
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in getting units of measurement for {type}", request.UnitType);
                throw;
            }
        }
    }
}
