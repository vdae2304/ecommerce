using Ecommerce.Common.Exceptions;
using Ecommerce.Common.Interfaces;
using Ecommerce.Common.Models.Orders;
using Ecommerce.Common.Models.Responses;
using Ecommerce.Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace Ecommerce.Controllers.Addresses.EditAddress
{
    public record EditAddressRequest : IRequest<IActionResult>
    {
        /// <summary>
        /// User ID linked to the address.
        /// </summary>
        [JsonIgnore]
        public int UserId { get; set; }

        /// <summary>
        /// Address ID.
        /// </summary>
        [JsonIgnore]
        public int AddressId { get; set; }

        /// <summary>
        /// Recipient name.
        /// </summary>
        [JsonRequired]
        public string Recipient { get; set; } = string.Empty;

        /// <summary>
        /// Contact phone number.
        /// </summary>
        [JsonRequired]
        public string Phone { get; set; } = string.Empty;

        /// <summary>
        /// Street name.
        /// </summary>
        [JsonRequired]
        public string Street { get; set; } = string.Empty;

        /// <summary>
        /// Street number.
        /// </summary>
        [JsonRequired]
        public string StreetNumber { get; set; } = string.Empty;

        /// <summary>
        /// Apartment number, if any.
        /// </summary>
        public string? AptNumber { get; set; }

        /// <summary>
        /// Neighbourhood.
        /// </summary>
        [JsonRequired]
        public string Neighbourhood { get; set; } = string.Empty;

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

                Address address = await _context.Addresses
                    .FirstOrDefaultAsync(x => x.Id == request.AddressId &&
                        x.UserId == request.UserId, cancellationToken)
                    ?? throw new NotFoundException($"Address {request.AddressId} does not exist");

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
