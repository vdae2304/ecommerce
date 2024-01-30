using Ecommerce.Common.Interfaces;
using Ecommerce.Common.Models.IAM;
using Ecommerce.Common.Models.Responses;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers.IAM.Password
{
    public class ForgotPasswordHandler : IRequestHandler<ForgotPasswordRequest, IActionResult>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailNotificationService _emailNotification;
        private readonly ILogger<ForgotPasswordHandler> _logger;

        public ForgotPasswordHandler(UserManager<ApplicationUser> userManager,
            IEmailNotificationService emailNotification, ILogger<ForgotPasswordHandler> logger)
        {
            _userManager = userManager;
            _emailNotification = emailNotification;
            _logger = logger;
        }

        public async Task<IActionResult> Handle(ForgotPasswordRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var validator = new ForgotPasswordValidator();
                await validator.ValidateAndThrowAsync(request, cancellationToken);

                ApplicationUser? user = await _userManager.FindByEmailAsync(request.Email);
                if (user != null)
                {
                    string token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    await _emailNotification.SendEmailAsync(new EmailMessage
                    {
                        Recipient = request.Email,
                        Subject = "Forgot password",
                        Body = "<p>Dear customer</p>"
                            + $"<p>Your token to create a new password is <em>{token}</em></p>"
                            + "<p>If you didn't request a new password, you can simply ignore this "
                            + "email. No further action is needed.</p>",
                        IsBodyHtml = true
                    });
                }

                return new OkObjectResult(new Response
                {
                    Success = true,
                    Message = "Ok."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in creating password reset token for {email}", request.Email);
                throw;
            }
        }
    }
}
