using Ecommerce.Common.Exceptions;
using Ecommerce.Common.Interfaces;
using Ecommerce.Common.Models.Orders;
using Ecommerce.Common.Models.Responses;
using Ecommerce.Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace Ecommerce.Controllers.Payment.EditPaymentMethod
{
    public record EditPaymentMethodRequest : IRequest<IActionResult>
    {
        /// <summary>
        /// User ID linked to the payment method.
        /// </summary>
        [JsonIgnore]
        public int UserId { get; set; }

        /// <summary>
        /// Payment method ID.
        /// </summary>
        [JsonIgnore]
        public int PaymentMethodId { get; set; }

        /// <summary>
        /// Card owner.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Card verification value.
        /// </summary>
        public string? CVV { get; set; }

        /// <summary>
        /// Month of expiration.
        /// </summary>
        public int? ExpiryMonth { get; set; }

        /// <summary>
        /// Year of expiration.
        /// </summary>
        public int? ExpiryYear { get; set; }
    }

    public class EditPaymentMethodHandler : IRequestHandler<EditPaymentMethodRequest, IActionResult>
    {
        private readonly ApplicationDbContext _context;
        private ISecurityManager _securityManager;
        private readonly ILogger<EditPaymentMethodHandler> _logger;

        public EditPaymentMethodHandler(ApplicationDbContext context, ISecurityManager securityManager, ILogger<EditPaymentMethodHandler> logger)
        {
            _context = context;
            _securityManager = securityManager;
            _logger = logger;
        }

        public async Task<IActionResult> Handle(EditPaymentMethodRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var validator = new EditPaymentMethodValidator();
                var validationResult = validator.Validate(request);
                if (!validationResult.IsValid)
                {
                    throw new BadRequestException(validationResult.ToString());
                }

                PaymentMethod paymentMethod = await _context.PaymentMethods
                    .FirstOrDefaultAsync(x => x.Id == request.PaymentMethodId &&
                        x.UserId == request.UserId, cancellationToken)
                    ?? throw new NotFoundException($"Payment method {request.PaymentMethodId} does not exist");

                if (request.Name != null)
                {
                    paymentMethod.Name = _securityManager.Encrypt(request.Name);
                }
                if (request.CVV != null)
                {
                    paymentMethod.CVV = _securityManager.Encrypt(request.CVV);
                }
                if (request.ExpiryMonth != null)
                {
                    paymentMethod.ExpiryMonth = _securityManager.Encrypt(request.ExpiryMonth.Value.ToString("00"));
                }
                if (request.ExpiryYear != null)
                {
                    paymentMethod.ExpiryYear = _securityManager.Encrypt(request.ExpiryYear.Value.ToString("0000"));
                }

                _context.PaymentMethods.Update(paymentMethod);
                await _context.SaveChangesAsync(cancellationToken);

                return new OkObjectResult(new Response
                {
                    Success = true,
                    Message = "Ok."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in editing payment method {paymentMethodId}", request.PaymentMethodId);
                throw;
            }
        }
    }
}
