using Ecommerce.Common.Exceptions;
using Ecommerce.Common.Interfaces;
using Ecommerce.Common.Models.Orders;
using Ecommerce.Common.Models.Responses;
using Ecommerce.Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace Ecommerce.Controllers.Payment.CreatePaymentMethod
{
    public record CreatePaymentMethodForm : IRequest<IActionResult>
    {
        /// <summary>
        /// User ID linked to the payment method.
        /// </summary>
        [JsonIgnore]
        public int UserId { get; set; }

        /// <summary>
        /// Card owner.
        /// </summary>
        [JsonRequired]
        public string Name { get; set; } = string.Empty;
 
        /// <summary>
        /// Card number.
        /// </summary>
        [JsonRequired]
        public string CardNumber { get; set; } = string.Empty;

        /// <summary>
        /// Card verification value.
        /// </summary>
        [JsonRequired]
        public string CVV { get; set; } = string.Empty;

        /// <summary>
        /// Month of expiration.
        /// </summary>
        [JsonRequired]
        public int ExpiryMonth { get; set; }

        /// <summary>
        /// Year of expiration.
        /// </summary>
        [JsonRequired]
        public int ExpiryYear { get; set; }
    }

    public class CreatePaymentMethodHandler : IRequestHandler<CreatePaymentMethodForm, IActionResult>
    {
        private readonly ApplicationDbContext _context;
        private readonly ISecurityManager _securityManager;
        private readonly ILogger<CreatePaymentMethodHandler> _logger;

        public CreatePaymentMethodHandler(ApplicationDbContext context, ISecurityManager securityManager,
            ILogger<CreatePaymentMethodHandler> logger)
        {
            _context = context;
            _securityManager = securityManager;
            _logger = logger;
        }

        public async Task<IActionResult> Handle(CreatePaymentMethodForm request, CancellationToken cancellationToken)
        {
            try
            {
                var validator = new CreatePaymentMethodValidator();
                var validationResult = validator.Validate(request);
                if (!validationResult.IsValid)
                {
                    throw new BadRequestException(validationResult.ToString());
                }

                var paymentMethod = new PaymentMethod
                {
                    UserId = request.UserId,
                    Name = _securityManager.Encrypt(request.Name),
                    CardNumber = _securityManager.Encrypt(request.CardNumber),
                    CVV = _securityManager.Encrypt(request.CVV),
                    ExpiryMonth = _securityManager.Encrypt(request.ExpiryMonth.ToString("00")),
                    ExpiryYear = _securityManager.Encrypt(request.ExpiryYear.ToString("0000")),
                };
                _context.PaymentMethods.Add(paymentMethod);
                await _context.SaveChangesAsync(cancellationToken);

                return new OkObjectResult(new Response<CreatedId>
                {
                    Success = true,
                    Message = "Ok.",
                    Data = new CreatedId { Id = paymentMethod.Id }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in creating payment method");
                throw;
            }
        }
    }
}
