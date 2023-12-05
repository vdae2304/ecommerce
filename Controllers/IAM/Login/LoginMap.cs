using AutoMapper;
using Ecommerce.Common.Models.IAM;
using Ecommerce.Common.Models.Responses;

namespace Ecommerce.Controllers.IAM.Login
{
    public class LoginMap : Profile
    {
        public LoginMap()
        {
            CreateMap<ApplicationUser, UserProfile>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));
        }
    }
}
