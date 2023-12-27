using Ecommerce.Common.Exceptions;
using Ecommerce.Common.Models.IAM;
using Ecommerce.Common.Models.Responses;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Controllers.IAM.ChangePassword
{
    public record PasswordRecoverRequest : IRequest<IActionResult>
    {
        /// <summary>
        /// Email address.
        /// </summary>
        [Required]
        public string Email { get; set; } = string.Empty;
        /// <summary>
        /// New password.
        /// </summary>
        [Required]
        public string NewPassword { get; set; } = string.Empty;
        /// <summary>
        /// Password reset token.
        /// </summary>
        [Required]
        public string Token { get; set; } = string.Empty;
    }

    public class PasswordRecoverHandler : IRequestHandler<PasswordRecoverRequest, IActionResult>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<PasswordRecoverHandler> _logger;

        public PasswordRecoverHandler(UserManager<ApplicationUser> userManager, ILogger<PasswordRecoverHandler> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<IActionResult> Handle(PasswordRecoverRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var validator = new PasswordRecoverValidator();
                var validationResult = validator.Validate(request);
                if (!validationResult.IsValid)
                {
                    throw new BadRequestException(validationResult.ToString());
                }

                ApplicationUser? user = await _userManager.FindByEmailAsync(request.Email);
                if (user != null)
                {
                    var result = await _userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);
                    if (!result.Succeeded)
                    {
                        throw new BadRequestException("Invalid token");
                    }
                }

                return new OkObjectResult(new Response
                {
                    Success = true,
                    Message = "Ok."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in changing password for {email}", request.Email);
                throw;
            }
        }
    }
}
