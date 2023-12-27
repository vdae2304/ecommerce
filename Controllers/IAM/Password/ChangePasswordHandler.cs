using Ecommerce.Common.Exceptions;
using Ecommerce.Common.Models.IAM;
using Ecommerce.Common.Models.Responses;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Controllers.IAM.Password
{
    public record ChangePasswordRequest : IRequest<IActionResult>
    {
        /// <summary>
        /// User ID.
        /// </summary>
        [BindNever]
        [FromRoute]
        public int UserId { get; set; }
        /// <summary>
        /// Current password.
        /// </summary>
        [Required]
        public string CurrentPassword { get; set; } = string.Empty;
        /// <summary>
        /// New password.
        /// </summary>
        [Required]
        public string NewPassword { get; set; } = string.Empty;
    }

    public class ChangePasswordHandler : IRequestHandler<ChangePasswordRequest, IActionResult>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<ChangePasswordHandler> _logger;

        public ChangePasswordHandler(UserManager<ApplicationUser> userManager, ILogger<ChangePasswordHandler> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<IActionResult> Handle(ChangePasswordRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var validator = new ChangePasswordValidator();
                var validationResult = validator.Validate(request);
                if (!validationResult.IsValid)
                {
                    throw new BadRequestException(validationResult.ToString());
                }

                ApplicationUser user = await _userManager.FindByIdAsync(request.UserId.ToString())
                    ?? throw new UnauthorizedException("Your username or password is incorrect");

                var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
                if (!result.Succeeded)
                {
                    throw new UnauthorizedException("Your username or password is incorrect");
                }

                return new OkObjectResult(new Response
                {
                    Success = true,
                    Message = "Ok."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in changing password for {userId}", request.UserId);
                throw;
            }
        }
    }
}
