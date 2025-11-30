using AutoMapper;
using Domain.Models.Product;
using Shared.ProductDtos;
namespace Services.Implementation.MappingProfiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDto>()
                .ForMember((dest) => dest.ProductBrand, (options) => options.MapFrom((source) => source.ProductBrand.Name))
                .ForMember((dest) => dest.ProductSubCategory, (options) => options.MapFrom((source) => source.ProductSubCategory.Name))
                .ForMember((dest) => dest.ProductImages, (options) => options.MapFrom<PictureURLResolver>());

            CreateMap<ProductImage, ProductImageDto>();
        }
    }
}
