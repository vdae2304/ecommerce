using Ecommerce.Common.Exceptions;
using Ecommerce.Common.Interfaces;
using Ecommerce.Common.Models.IAM;
using Ecommerce.Common.Models.Responses;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;

namespace Ecommerce.Controllers.IAM.Login
{
    public record LoginRequest : IRequest<IActionResult>
    {
        /// <summary>
        /// Username.
        /// </summary>
        [Required]
        public string UserName { get; set; } = string.Empty;
        /// <summary>
        /// Password.
        /// </summary>
        [Required]
        public string Password { get; set; } = string.Empty;
    }

    public class LoginHandler : IRequestHandler<LoginRequest, IActionResult>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ISecurityManager _securityManager;
        private readonly ILogger<LoginHandler> _logger;

        public LoginHandler(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, ISecurityManager securityManager,
            ILogger<LoginHandler> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _securityManager = securityManager;
            _logger = logger;
        }

        public async Task<IActionResult> Handle(LoginRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(request.UserName)
                    ?? throw new UnauthorizedException("Your username or password is incorrect");

                var result = await _signInManager.PasswordSignInAsync(user, request.Password, false, false);
                if (!result.Succeeded)
                {
                    throw new UnauthorizedException("Your username or password is incorrect");
                }

                return new OkObjectResult(new Response<Authentication>
                {
                    Success = true,
                    Message = "Ok.",
                    Data = _securityManager.GenerateAccessToken(user)
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in login user {userName}", request.UserName);
                throw;
            }
        }
    }
}
