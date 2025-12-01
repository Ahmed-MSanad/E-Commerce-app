//using AutoMapper;
//using Domain.Models.Basket;
//using Microsoft.Extensions.Configuration;
//using Shared.BasketDtos;
//using Shared.ProductDtos;

//namespace Services.Implementation.MappingProfiles
//{
//    public class PictureURLBasketResolver(IConfiguration configuration) : IValueResolver<BasketItem, BasketItemDto, List<ProductImageDto>>
//    {
//        public List<ProductImageDto> Resolve(BasketItem source, BasketItemDto destination, List<ProductImageDto> destMember, ResolutionContext context)
//        {
//            return source.ProductImages.Select(PImage =>
//            {
//                if (string.IsNullOrWhiteSpace(PImage.Image))
//                    return new ProductImageDto { Image = string.Empty };
//                return new ProductImageDto { Image = $"{configuration["BaseUrl"]}{PImage.Image}" };
//            }).ToList();
//        }
//    }
//}
