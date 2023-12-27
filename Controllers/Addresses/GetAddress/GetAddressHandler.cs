using Ecommerce.Common.Exceptions;
using Ecommerce.Common.Interfaces;
using Ecommerce.Common.Models.Orders;
using Ecommerce.Common.Models.Responses;
using Ecommerce.Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Controllers.Addresses.GetAddress
{
    public record GetAddressRequest : IRequest<IActionResult>
    {
        /// <summary>
        /// User ID linked to the address.
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// Address ID.
        /// </summary>
        public int AddressId { get; set; }
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
                Address address = await _context.Addresses
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == request.AddressId &&
                        x.UserId == request.UserId, cancellationToken)
                    ?? throw new NotFoundException($"Address {request.AddressId} does not exist");

                address.Recipient = _securityManager.Decrypt(address.Recipient);
                address.Phone = _securityManager.Decrypt(address.Phone);

                return new OkObjectResult(new Response<Address>
                {
                    Success = true,
                    Message = "Ok.",
                    Data = address
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in getting details for address {addressId}", request.AddressId);
                throw;
            }
        }
    }
}
