using Ecommerce.Common.Exceptions;
using Ecommerce.Common.Interfaces;
using Ecommerce.Common.Models.Responses;
using Ecommerce.Common.Models.Schema;
using Ecommerce.Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Controllers.Units.DeleteUnit
{
    public record DeleteUnitRequest : IRequest<IActionResult>
    {
        /// <summary>
        /// Unit ID.
        /// </summary>
        public int UnitId;
    }

    public class DeleteUnitHandler : IRequestHandler<DeleteUnitRequest, IActionResult>
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DeleteUnitHandler> _logger;

        public DeleteUnitHandler(ApplicationDbContext context, ILogger<DeleteUnitHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Handle(DeleteUnitRequest request, CancellationToken cancellationToken)
        {
            try
            {
                MeasureUnit unit = await _context.MeasureUnits
                    .FirstOrDefaultAsync(x => x.Id == request.UnitId, cancellationToken)
                    ?? throw new NotFoundException($"Unit {request.UnitId} does not exist");
                
                _context.MeasureUnits.Remove(unit);
                await _context.SaveChangesAsync(cancellationToken);

                return new OkObjectResult(new Response
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
