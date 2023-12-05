using AutoMapper;
using Ecommerce.Common.Exceptions;
using Ecommerce.Common.Models.IAM;
using Ecommerce.Common.Models.Responses;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Controllers.IAM.SignUp
{
    public record SignUpRequest : IRequest<IActionResult>
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
        /// <summary>
        /// Email address.
        /// </summary>
        [Required]
        public string Email { get; set; } = string.Empty;
    }

    public class SignUpHandler : IRequestHandler<SignUpRequest, IActionResult>
    {
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<SignUpHandler> _logger;

        public SignUpHandler(IMapper mapper, UserManager<ApplicationUser> userManager,
            ILogger<SignUpHandler> logger)
        {
            _mapper = mapper;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<IActionResult> Handle(SignUpRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var validator = new SignUpValidator(_userManager);
                var validationResult = await validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    throw new BadRequestException(validationResult.ToString());
                }

                ApplicationUser user = _mapper.Map<ApplicationUser>(request);

                var result = await _userManager.CreateAsync(user, request.Password);
                if (!result.Succeeded)
                {
                    throw new BadRequestException(result.ToString());
                }

                return new OkObjectResult(new Response<UserProfile>
                {
                    Success = true,
                    Message = "Ok.",
                    Data = _mapper.Map<UserProfile>(user)
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in creating user {userName}", request.UserName);
                throw;
            }
        }
    }
}
