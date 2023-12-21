using Ecommerce.Common.Exceptions;
using Ecommerce.Common.Models.Orders;
using Ecommerce.Common.Models.Responses;
using Ecommerce.Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Controllers.Shipping.DeleteAddress
{
    public record DeleteAddressRequest : IRequest<IActionResult>
    {
        /// <summary>
        /// User ID linked to the address.
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// Shipping address ID.
        /// </summary>
        public int ShippingAddressId { get; set; }
    }

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
                ShippingAddress shippingAddress = await _context.ShippingAddresses
                    .FirstOrDefaultAsync(x => x.Id == request.ShippingAddressId &&
                        x.UserId == request.UserId, cancellationToken)
                    ?? throw new NotFoundException($"Shipping address {request.ShippingAddressId} does not exist");

                _context.ShippingAddresses.Remove(shippingAddress);
                await _context.SaveChangesAsync(cancellationToken);

                return new OkObjectResult(new Response
                {
                    Success = true,
                    Message = "Ok."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in deleting shipping address {shippingAddressId}", request.ShippingAddressId);
                throw;
            }
        }
    }
}
