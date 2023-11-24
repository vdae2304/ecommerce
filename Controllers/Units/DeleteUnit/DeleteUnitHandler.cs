using Ecommerce.Common.Exceptions;
using Ecommerce.Common.Interfaces;
using Ecommerce.Common.Models.Responses;
using Ecommerce.Common.Models.Schema;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers.Units.DeleteUnit
{
    public record DeleteUnitRequest : IRequest<ActionResult>
    {
        /// <summary>
        /// Unit ID.
        /// </summary>
        public int UnitId;
    }

    public class DeleteUnitHandler : IRequestHandler<DeleteUnitRequest, ActionResult>
    {
        private readonly IGenericRepository<MeasureUnit> _units;
        private readonly ILogger<DeleteUnitHandler> _logger;

        public DeleteUnitHandler(IGenericRepository<MeasureUnit> units, ILogger<DeleteUnitHandler> logger)
        {
            _units = units;
            _logger = logger;
        }

        public async Task<ActionResult> Handle(DeleteUnitRequest request, CancellationToken cancellationToken)
        {
            try
            {
                MeasureUnit unit = await _units.FindByIdAsync(request.UnitId, cancellationToken)
                    ?? throw new NotFoundException($"Unit {request.UnitId} does not exist");
                
                await _units.DeleteAsync(unit, cancellationToken);

                return new OkObjectResult(new StatusResponse
                {
                    Success = true,
                    Message = "OK."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in deleting unit of measurement {id}", request.UnitId);
                throw;
            }
        }
    }
}
