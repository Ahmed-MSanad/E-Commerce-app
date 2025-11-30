using AutoMapper;
using Domain.Contracts;
using Domain.Models.Product;
using Services.Abstraction;
using Services.Implementation.Specifications;
using Shared.ProductDtos;

namespace Services.Implementation
{
    public class ProductService(IUnitOfWork unitOfWork, IMapper mapper) : IProductService
    {
        public async Task<IEnumerable<ProductDto>> GetAllProducts(ProductSpecificationParams productSpecificationParams)
        {
            productSpecificationParams.search ??= string.Empty;
            
            ProductWithRelatedDataSpecification productSpecification = new ProductWithRelatedDataSpecification(productSpecificationParams);
            
            var products = await unitOfWork.GetRepository<Product, int>().GetAllAsync(productSpecification);

            if(products is null || !products.Any())
                throw new Exception("GetAllProducts -> No Products found");

            return mapper.Map<IEnumerable<ProductDto>>(products);
        }
        public async Task<ProductDto> GetProductById(int id)
        {
            ProductWithRelatedDataSpecification getPtoductWithRelatedData = new ProductWithRelatedDataSpecification(id);
            
            var product = await unitOfWork.GetRepository<Product, int>().GetAsync(getPtoductWithRelatedData);
            
            if (product is null)
                throw new Exception($"GetProductById -> No Product found with id: {id}");

            return mapper.Map<ProductDto>(product);
        }
    }
}
