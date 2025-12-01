using AutoMapper;
using Domain.Models.Product;
using Shared.ProductDtos;
using Microsoft.Extensions.Configuration;

namespace Services.Implementation.MappingProfiles
{
    public class PictureURLResolver(IConfiguration configuration) : IValueResolver<Product, ProductDto, List<ProductImageDto>>
    {
        public List<ProductImageDto> Resolve(Product source, ProductDto destination, List<ProductImageDto> destMember, ResolutionContext context)
        {
            return source.ProductImages.Select(PImage =>
            {
                if (string.IsNullOrWhiteSpace(PImage.Image))
                    return new ProductImageDto { Image = string.Empty };
                return new ProductImageDto { Image = $"{configuration["BaseUrl"]}{PImage.Image}" };
            }).ToList();
        }
    }
}
