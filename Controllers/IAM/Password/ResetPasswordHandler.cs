using Ecommerce.Common.Exceptions;
using Ecommerce.Common.Models.IAM;
using Ecommerce.Common.Models.Responses;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers.IAM.Password
{
    public class ResetPasswordHandler : IRequestHandler<ResetPasswordRequest, IActionResult>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<ResetPasswordHandler> _logger;

        public ResetPasswordHandler(UserManager<ApplicationUser> userManager, ILogger<ResetPasswordHandler> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<IActionResult> Handle(ResetPasswordRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var validator = new ResetPasswordValidator();
                await validator.ValidateAndThrowAsync(request, cancellationToken);

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
