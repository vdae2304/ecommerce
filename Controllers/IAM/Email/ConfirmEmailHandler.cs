using Ecommerce.Common.Exceptions;
using Ecommerce.Common.Models.IAM;
using Ecommerce.Common.Models.Responses;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers.IAM.Email
{
    public class ConfirmEmailHandler : IRequestHandler<ConfirmEmailRequest, IActionResult>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<ConfirmEmailHandler> _logger;

        public ConfirmEmailHandler(UserManager<ApplicationUser> userManager, ILogger<ConfirmEmailHandler> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<IActionResult> Handle(ConfirmEmailRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var validator = new ConfirmEmailValidator();
                await validator.ValidateAndThrowAsync(request, cancellationToken);

                ApplicationUser user = await _userManager.FindByEmailAsync(request.Email)
                    ?? throw new BadRequestException("Invalid email");

                var result = await _userManager.ConfirmEmailAsync(user, request.Token);
                if (!result.Succeeded)
                {
                    throw new BadRequestException("Invalid token");
                }

                return new OkObjectResult(new Response
                {
                    Success = true,
                    Message = "Ok."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in verifying email {email}", request.Email);
                throw;
            }
        }
    }
}
