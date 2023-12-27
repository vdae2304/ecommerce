using Ecommerce.Common.Exceptions;
using Ecommerce.Common.Models.IAM;
using Ecommerce.Common.Models.Orders;
using Ecommerce.Common.Models.Responses;
using Ecommerce.Common.Models.Schema;
using Ecommerce.Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace Ecommerce.Controllers.Carts.AddProduct
{
    public record AddProductRequest : IRequest<IActionResult>
    {
        /// <summary>
        /// User ID who owns the cart.
        /// </summary>
        [JsonIgnore]
        public int UserId { get; set; }
        /// <summary>
        /// Product ID to add to the cart.
        /// </summary>
        public int ProductId { get; set; }
        /// <summary>
        /// Number of products to add to the cart.
        /// </summary>
        public int Quantity { get; set; }
    }

    public class AddProductHandler : IRequestHandler<AddProductRequest, IActionResult>
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<AddProductHandler> _logger;

        public AddProductHandler(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
            ILogger<AddProductHandler> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<IActionResult> Handle(AddProductRequest request, CancellationToken cancellationToken)
        {
            try
            {
                ApplicationUser user = await _userManager.FindByIdAsync(request.UserId.ToString())
                    ?? throw new UnauthorizedException("");
                Product product = await _context.Products
                    .FirstOrDefaultAsync(x => x.Id == request.ProductId, cancellationToken)
                    ?? throw new BadRequestException($"Product {request.ProductId} does not exist");

                if (request.Quantity < product.MinPurchaseQuantity)
                {
                    throw new BadRequestException($"Field Quantity cannot be less than {product.MinPurchaseQuantity}");
                }
                if (request.Quantity > product.MaxPurchaseQuantity)
                {
                    throw new BadRequestException($"Field Quantity cannot be greater than {product.MaxPurchaseQuantity}");
                }
                if (!product.Unlimited && request.Quantity > product.InStock)
                {
                    throw new BadRequestException("Not enough stock");
                }

                _context.Carts.Add(new Cart
                {
                    UserId = request.UserId,
                    Product = product,
                    Quantity = request.Quantity
                });
                await _context.SaveChangesAsync(cancellationToken);

                return new OkObjectResult(new Response
                {
                    Success = true,
                    Message = "Ok."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in adding product {productId} to cart", request.ProductId);
                throw;
            }
        }
    }
}
