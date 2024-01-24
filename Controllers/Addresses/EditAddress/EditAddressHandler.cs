using Ecommerce.Common.Exceptions;
using Ecommerce.Common.Interfaces;
using Ecommerce.Common.Models.Orders;
using Ecommerce.Common.Models.Responses;
using Ecommerce.Infrastructure.Data;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Controllers.Addresses.EditAddress
{
    public class EditAddressHandler : IRequestHandler<EditAddressRequest, IActionResult>
    {
        private readonly ApplicationDbContext _context;
        private readonly ISecurityManager _securityManager;
        private readonly ILogger<EditAddressHandler> _logger;

        public EditAddressHandler(ApplicationDbContext context, ISecurityManager securityManager,
            ILogger<EditAddressHandler> logger)
        {
            _context = context;
            _securityManager = securityManager;
            _logger = logger;
        }

        public async Task<IActionResult> Handle(EditAddressRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var validator = new EditAddressValidator();
                await validator.ValidateAndThrowAsync(request, cancellationToken);

                Address address = await _context.Addresses
                    .FirstOrDefaultAsync(x => x.Id == request.AddressId &&
                        x.UserId == request.UserId, cancellationToken)
                    ?? throw new NotFoundException();

                address.Recipient = _securityManager.Encrypt(request.Recipient);
                address.Phone = _securityManager.Encrypt(request.Phone);
                address.Street = request.Street;
                address.StreetNumber = request.StreetNumber;
                address.Neighbourhood = request.Neighbourhood;
                address.City = request.City;
                address.State = request.State;
                address.PostalCode = request.PostalCode;
                address.Comments = request.Comments;

                _context.Addresses.Update(address);
                await _context.SaveChangesAsync(cancellationToken);

                return new OkObjectResult(new Response
                {
                    Success = true,
                    Message = "Ok."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in editing address {addressId}", request.AddressId);
                throw;
            }
        }
    }
}
