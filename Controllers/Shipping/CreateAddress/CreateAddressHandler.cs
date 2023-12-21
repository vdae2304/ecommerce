using Ecommerce.Common.Exceptions;
using Ecommerce.Common.Interfaces;
using Ecommerce.Common.Models.Orders;
using Ecommerce.Common.Models.Responses;
using Ecommerce.Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace Ecommerce.Controllers.Shipping.CreateAddress
{
    public record CreateAddressForm : IRequest<IActionResult>
    {
        /// <summary>
        /// User ID linked to the address.
        /// </summary>
        [JsonIgnore]
        public int UserId { get; set; }

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

    public class CreateAddressHandler : IRequestHandler<CreateAddressForm, IActionResult>
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

        public async Task<IActionResult> Handle(CreateAddressForm request, CancellationToken cancellationToken)
        {
            try
            {
                var validator = new CreateAddressValidator();
                var validationResult = validator.Validate(request);
                if (!validationResult.IsValid)
                {
                    throw new BadRequestException(validationResult.ToString());
                }

                var shippingAddress = new ShippingAddress
                {
                    UserId = request.UserId,
                    Name = _securityManager.Encrypt(request.Name),
                    Phone = _securityManager.Encrypt(request.Phone),
                    Street = request.Street,
                    City = request.City,
                    State = request.State,
                    PostalCode = request.PostalCode,
                    Comments = request.Comments
                };

                _context.ShippingAddresses.Add(shippingAddress);
                await _context.SaveChangesAsync(cancellationToken);

                return new OkObjectResult(new Response<CreatedId>
                {
                    Success = true,
                    Message = "Ok.",
                    Data = new CreatedId { Id = shippingAddress.Id }
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
