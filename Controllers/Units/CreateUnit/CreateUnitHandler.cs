using AutoMapper;
using Ecommerce.Common.Exceptions;
using Ecommerce.Common.Models.Responses;
using Ecommerce.Common.Models.Schema;
using Ecommerce.Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Controllers.Units.CreateUnit
{
    public record CreateUnitForm : IRequest<ActionResult>
    {
        /// <summary>
        /// Unit symbol.
        /// </summary>
        [Required]
        public string Symbol { get; set; } = string.Empty;
        
        /// <summary>
        /// Unit name.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Unit type.
        /// </summary>
        [Required]
        public MeasureUnitType Type { get; set; }
    }

    public class CreateUnitHandler : IRequestHandler<CreateUnitForm, IActionResult>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateUnitHandler> _logger;

        public CreateUnitHandler(ApplicationDbContext context, IMapper mapper, ILogger<CreateUnitHandler> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IActionResult> Handle(CreateUnitForm request, CancellationToken cancellationToken)
        {
            try
            {
                var validator = new CreateUnitValidator(_context);
                var validationResult = await validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    throw new BadRequestException(validationResult.ToString());
                }

                MeasureUnit unit = _mapper.Map<MeasureUnit>(request);
                _context.MeasureUnits.Add(unit);
                await _context.SaveChangesAsync(cancellationToken);

                return new OkObjectResult(new Response
                {
                    Success = true,
                    Message = $"Unit created with id {unit.Id}"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in creating unit of measurement {symbol}", request.Symbol);
                throw;
            }
        }
    }
}
