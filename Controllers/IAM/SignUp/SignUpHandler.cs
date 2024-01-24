using AutoMapper;
using Ecommerce.Common.Exceptions;
using Ecommerce.Common.Models.IAM;
using Ecommerce.Common.Models.Responses;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers.IAM.SignUp
{
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
                await validator.ValidateAndThrowAsync(request, cancellationToken);

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
