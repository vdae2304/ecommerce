using Ecommerce.Common.Interfaces;
using Ecommerce.Common.Models.Orders;
using Ecommerce.Common.Models.Responses;
using Ecommerce.Infrastructure.Data;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers.Addresses.CreateAddress
{
    public class CreateAddressHandler : IRequestHandler<CreateAddressRequest, IActionResult>
    {
        private readonly ApplicationDbContext _context;
        private readonly ISecurityManager _securityManager;
        private readonly ILogger<CreateAddressHandler> _logger;

        public CreateAddressHandler(ApplicationDbContext context, ISecurityManager securityManager,
            ILogger<CreateAddressHandler> logger)
        {
            _context = context;
            _securityManager = securityManager;
            _logger = logger;
        }

        public async Task<IActionResult> Handle(CreateAddressRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var validator = new CreateAddressValidator();
                await validator.ValidateAndThrowAsync(request, cancellationToken);

                var address = new Address
                {
                    UserId = request.UserId,
                    Recipient = _securityManager.Encrypt(request.Recipient),
                    Phone = _securityManager.Encrypt(request.Phone),
                    Street = request.Street,
                    StreetNumber = request.StreetNumber,
                    AptNumber = request.AptNumber,
                    Neighbourhood = request.Neighbourhood,
                    City = request.City,
                    State = request.State,
                    PostalCode = request.PostalCode,
                    Comments = request.Comments
                };

                _context.Addresses.Add(address);
                await _context.SaveChangesAsync(cancellationToken);

                return new OkObjectResult(new Response<CreatedId>
                {
                    Success = true,
                    Message = "Ok.",
                    Data = new CreatedId { Id = address.Id }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in creating address");
                throw;
            }
        }
    }
}
