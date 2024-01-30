using Ecommerce.Common.Exceptions;
using Ecommerce.Common.Models.Orders;
using Ecommerce.Common.Models.Responses;
using Ecommerce.Common.Models.Schema;
using Ecommerce.Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Controllers.Orders.CreateOrder
{
    public class CreateOrderHandler : IRequestHandler<CreateOrderRequest, IActionResult>
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CreateOrderHandler> _logger;

        public CreateOrderHandler(ApplicationDbContext context, ILogger<CreateOrderHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Handle(CreateOrderRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var errors = new List<string>();

                var items = new List<OrderProducts>();
                foreach (var item in request.Items)
                {
                    Product? product = await _context.Products
                        .FirstOrDefaultAsync(x => x.Id == item.ProductId, cancellationToken);

                    if (product == null)
                    {
                        errors.Add($"Product {item.ProductId} does not exist");
                        continue;
                    }
                    if (!product.Enabled)
                    {
                        errors.Add($"Product {product.Id} is not available at the moment");
                        continue;
                    }

                    if (item.Quantity < product.MinPurchaseQuantity ||
                        item.Quantity > product.MaxPurchaseQuantity)
                    {
                        errors.Add($"Quantity must be between {product.MinPurchaseQuantity} and"
                            + $" {product.MaxPurchaseQuantity} for product {product.Id}");
                    }
                    if (!product.Unlimited && item.Quantity > product.InStock)
                    {
                        errors.Add($"Not enough stock for product {product.Id}");
                    }

                    items.Add(new OrderProducts
                    {
                        Product = product,
                        Price = product.Price,
                        Quantity = item.Quantity
                    });
                }

                PaymentMethod? paymentMethod = await _context.PaymentMethods
                    .FirstOrDefaultAsync(x => x.Id == request.PaymentMethodId &&
                        x.UserId == request.UserId, cancellationToken);
                if (paymentMethod == null)
                {
                    errors.Add($"Payment method {request.PaymentMethodId} does not exist");
                }

                Address? shippingAddress = await _context.Addresses
                    .FirstOrDefaultAsync(x => x.Id == request.ShippingAddressId &&
                        x.UserId == request.UserId, cancellationToken);
                if (shippingAddress == null)
                {
                    errors.Add($"Address {request.ShippingAddressId} does not exist");
                }

                if (errors.Any())
                {
                    throw new BadRequestException(errors);
                }

                decimal subtotal = items.Sum(x => x.Subtotal);
                decimal shippingCost = 0;
                decimal total = subtotal + shippingCost;

                var order = new Order
                {
                    UserId = request.UserId,
                    Items = items,
                    Subtotal = subtotal,
                    PaymentMethod = paymentMethod,
                    PaymentStatus = PaymentStatus.AwaitingPayment,
                    ShippingAddress = shippingAddress,
                    ShippingStatus = ShippingStatus.AwaitingProcessing,
                    ShippingCost = shippingCost,
                    TrackingNumber = string.Empty,
                    Total = total
                };

                _context.Orders.Add(order);
                await _context.SaveChangesAsync(cancellationToken);

                return new OkObjectResult(new Response<CreatedId>
                {
                    Success = true,
                    Message = "Ok.",
                    Data = new CreatedId { Id = order.Id }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in creating request for user {userId}", request.UserId);
                throw;
            }
        }
    }
}
