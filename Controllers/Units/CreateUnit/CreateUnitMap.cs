using AutoMapper;
using Ecommerce.Common.Models.Schema;

namespace Ecommerce.Controllers.Units.CreateUnit
{
    public class CreateUnitMap : Profile
    {
        public CreateUnitMap()
        {
            CreateMap<CreateUnitForm, MeasureUnit>()
                .ForMember(dest => dest.Symbol, opt => opt.MapFrom(src => src.Symbol))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type));
        }
    }
}
