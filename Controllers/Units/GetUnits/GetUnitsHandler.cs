using Ecommerce.Common.Interfaces;
using Ecommerce.Common.Models.Responses;
using Ecommerce.Common.Models.Schema;
using Ecommerce.Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Controllers.Units.GetUnits
{
    public record GetUnitsRequest : IRequest<IActionResult>
    {
        /// <summary>
        /// Unit type.
        /// </summary>
        public MeasureUnitType UnitType;
    }

    public class GetUnitsHandler : IRequestHandler<GetUnitsRequest, IActionResult>
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<GetUnitsHandler> _logger;

        public GetUnitsHandler(ApplicationDbContext context, ILogger<GetUnitsHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Handle(GetUnitsRequest request, CancellationToken cancellationToken)
        {
            try
            {
                List<MeasureUnit> units = await _context.MeasureUnits
                    .Where(x => x.Type == request.UnitType)
                    .ToListAsync(cancellationToken);

                return new OkObjectResult(new Response<List<MeasureUnit>>
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
