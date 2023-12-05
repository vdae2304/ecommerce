using AutoMapper;
using Ecommerce.Common.Models.IAM;
using Ecommerce.Common.Models.Responses;

namespace Ecommerce.Controllers.IAM.SignUp
{
    public class SignUpMap : Profile
    {
        public SignUpMap()
        {
            CreateMap<SignUpRequest, ApplicationUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));
        }
    }
}
