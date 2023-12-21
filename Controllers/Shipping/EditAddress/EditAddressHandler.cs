using Ecommerce.Common.Exceptions;
using Ecommerce.Common.Interfaces;
using Ecommerce.Common.Models.Orders;
using Ecommerce.Common.Models.Responses;
using Ecommerce.Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace Ecommerce.Controllers.Shipping.EditAddress
{
    public record EditAddressRequest : IRequest<IActionResult>
    {
        /// <summary>
        /// User ID linked to the address.
        /// </summary>
        [JsonIgnore]
        public int UserId { get; set; }

        /// <summary>
        /// Shipping address ID.
        /// </summary>
        [JsonIgnore]
        public int ShippingAddressId { get; set; }

        /// <summary>
        /// Contact name.
        /// </summary>
        [JsonRequired]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Contact phone number.
        /// </summary>
        [JsonRequired]
        public string Phone { get; set; } = string.Empty;

        /// <summary>
        /// Street
        /// </summary>
        [JsonRequired]
        public string Street { get; set; } = string.Empty;

        /// <summary>
        /// City.
        /// </summary>
        [JsonRequired]
        public string City { get; set; } = string.Empty;

        /// <summary>
        /// State/province code.
        /// </summary>
        [JsonRequired]
        public string State { get; set; } = string.Empty;

        /// <summary>
        /// Postal code.
        /// </summary>
        [JsonRequired]
        public string PostalCode { get; set; } = string.Empty;

        /// <summary>
        /// Additional comments.
        /// </summary>
        public string? Comments { get; set; }
    }

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
                var validationResult = validator.Validate(request);
                if (!validationResult.IsValid)
                {
                    throw new BadRequestException(validationResult.ToString());
                }

                ShippingAddress shippingAddress = await _context.ShippingAddresses
                    .FirstOrDefaultAsync(x => x.Id == request.ShippingAddressId &&
                        x.UserId == request.UserId, cancellationToken)
                    ?? throw new NotFoundException($"Shipping address {request.ShippingAddressId} does not exist");

                shippingAddress.Name = _securityManager.Encrypt(request.Name);
                shippingAddress.Phone = _securityManager.Encrypt(request.Phone);
                shippingAddress.Street = request.Street;
                shippingAddress.City = request.City;
                shippingAddress.State = request.State;
                shippingAddress.PostalCode = request.PostalCode;
                shippingAddress.Comments = request.Comments;

                _context.ShippingAddresses.Update(shippingAddress);
                await _context.SaveChangesAsync(cancellationToken);

                return new OkObjectResult(new Response
                {
                    Success = true,
                    Message = "Ok."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in editing shipping address {shippingAddressId}", request.ShippingAddressId);
                throw;
            }
        }
    }
}
