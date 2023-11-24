using AutoMapper;
using Ecommerce.Common.Models.Schema;

namespace Ecommerce.Controllers.Products.CreateProduct
{
    public class CreateProductMap : Profile
    {
        public CreateProductMap()
        {
            CreateMap<CreateProductForm, Product>()
                .ForMember(dest => dest.Sku, opt => opt.MapFrom(src => src.Sku))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.Width, opt => opt.MapFrom(src => src.Width))
                .ForMember(dest => dest.Height, opt => opt.MapFrom(src => src.Height))
                .ForMember(dest => dest.Length, opt => opt.MapFrom(src => src.Length))
                .ForMember(dest => dest.DimensionUnitsId, opt => opt.MapFrom(src => src.DimensionUnitsId))
                .ForMember(dest => dest.Weight, opt => opt.MapFrom(src => src.Weight))
                .ForMember(dest => dest.WeightUnitsId, opt => opt.MapFrom(src => src.WeightUnitsId))
                .ForMember(dest => dest.Volume, opt => opt.MapFrom(src => src.Volume))
                .ForMember(dest => dest.VolumeUnitsId, opt => opt.MapFrom(src => src.VolumeUnitsId))
                .ForMember(dest => dest.MinPurchaseQuantity, opt => opt.MapFrom(src => src.MinPurchaseQuantity))
                .ForMember(dest => dest.MaxPurchaseQuantity, opt => opt.MapFrom(src => src.MaxPurchaseQuantity))
                .ForMember(dest => dest.InStock, opt => opt.MapFrom(src => src.InStock));
        }
    }

    public class CreateAttributeMap : Profile
    {
        public CreateAttributeMap()
        {
            CreateMap<CreateAttributeForm, ProductAttribute>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Value));
        }
    }
}
