using Ecommerce.Common.Exceptions;
using Ecommerce.Common.Interfaces;
using Ecommerce.Common.Models.Orders;
using Ecommerce.Common.Models.Responses;
using Ecommerce.Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Controllers.Shipping.GetAddress
{
    public record GetAddressRequest : IRequest<IActionResult>
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

    public class GetAddressHandler : IRequestHandler<GetAddressRequest, IActionResult>
    {
        private readonly ApplicationDbContext _context;
        private readonly ISecurityManager _securityManager;
        private readonly ILogger<GetAddressHandler> _logger;

        public GetAddressHandler(ApplicationDbContext context, ISecurityManager securityManager,
            ILogger<GetAddressHandler> logger)
        {
            _context = context;
            _securityManager = securityManager;
            _logger = logger;
        }

        public async Task<IActionResult> Handle(GetAddressRequest request, CancellationToken cancellationToken)
        {
            try
            {
                ShippingAddress shippingAddress = await _context.ShippingAddresses
                    .FirstOrDefaultAsync(x => x.Id == request.ShippingAddressId &&
                        x.UserId == request.UserId, cancellationToken)
                    ?? throw new NotFoundException($"Shipping address {request.ShippingAddressId} does not exist");

                shippingAddress.Name = _securityManager.Decrypt(shippingAddress.Name);
                shippingAddress.Phone = _securityManager.Decrypt(shippingAddress.Phone);

                return new OkObjectResult(new Response<ShippingAddress>
                {
                    Success = true,
                    Message = "Ok.",
                    Data = shippingAddress
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in getting details for shipping address {shippingAddressId}", request.ShippingAddressId);
                throw;
            }
        }
    }
}
