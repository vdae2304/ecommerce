using AutoMapper;
using Ecommerce.Common.Models.Schema;

namespace Ecommerce.Controllers.Categories.CreateCategory
{
    public class CreateCategoryMap : Profile
    {
        public CreateCategoryMap()
        {
            CreateMap<CreateCategoryForm, Category>()
                .ForMember(dest => dest.ParentId, opt => opt.MapFrom(src => src.ParentId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));
        }
    }
}
