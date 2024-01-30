using Ecommerce.Common.Exceptions;
using Ecommerce.Common.Interfaces;
using Ecommerce.Common.Models.IAM;
using Ecommerce.Common.Models.Responses;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers.IAM.Email
{
    public class SendConfirmationHandler : IRequestHandler<SendConfirmationRequest, IActionResult>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailNotificationService _emailNotification;
        private readonly ILogger<SendConfirmationHandler> _logger;

        public SendConfirmationHandler(UserManager<ApplicationUser> userManager,
            IEmailNotificationService emailNotification, ILogger<SendConfirmationHandler> logger)
        {
            _userManager = userManager;
            _emailNotification = emailNotification;
            _logger = logger;
        }

        public async Task<IActionResult> Handle(SendConfirmationRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var validator = new SendConfirmationValidator();
                await validator.ValidateAndThrowAsync(request, cancellationToken);

                ApplicationUser user = await _userManager.FindByEmailAsync(request.Email)
                    ?? throw new BadRequestException("Invalid email");

                if (await _userManager.IsEmailConfirmedAsync(user))
                {
                    throw new BadRequestException("Email already verified");
                }
                else {
                    string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    await _emailNotification.SendEmailAsync(new EmailMessage
                    {
                        Recipient = request.Email,
                        Subject = "Email confirmation",
                        Body = "<p>Dear customer</p>"
                            + $"<p>Your token to confirm your email is <em>{token}</em></p>",
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
                _logger.LogError(ex, "Error in sending email confirmation code to {email}", request.Email);
                throw;
            }
        }
    }
}
