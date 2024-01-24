using Ecommerce.Common.Exceptions;
using Ecommerce.Common.Models.Orders;
using Ecommerce.Common.Models.Responses;
using Ecommerce.Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Controllers.Addresses.DeleteAddress
{
    public class DeleteAddressHandler : IRequestHandler<DeleteAddressRequest, IActionResult>
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DeleteAddressHandler> _logger;

        public DeleteAddressHandler(ApplicationDbContext context, ILogger<DeleteAddressHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Handle(DeleteAddressRequest request, CancellationToken cancellationToken)
        {
            try
            {
                Address address = await _context.Addresses
                    .FirstOrDefaultAsync(x => x.Id == request.AddressId &&
                        x.UserId == request.UserId, cancellationToken)
                    ?? throw new NotFoundException();

                _context.Addresses.Remove(address);
                await _context.SaveChangesAsync(cancellationToken);

                return new OkObjectResult(new Response
                {
                    Success = true,
                    Message = "Ok."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in deleting address {addressId}", request.AddressId);
                throw;
            }
        }
    }
}
