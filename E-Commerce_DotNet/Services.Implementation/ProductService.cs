using AutoMapper;
using Domain.Contracts;
using Domain.Models.Product;
using Services.Abstraction;
using Services.Implementation.Specifications;
using Shared;
using Shared.ProductDtos;

namespace Services.Implementation
{
    public class ProductService(IUnitOfWork unitOfWork, IMapper mapper) : IProductService
    {
        public async Task<PaginatedResult<ProductDto>> GetAllProducts(ProductSpecificationParams productSpecificationParams)
        {
            ProductWithRelatedDataSpecification productSpecification = new ProductWithRelatedDataSpecification(productSpecificationParams);
            IEnumerable<Product> products = await unitOfWork.GetRepository<Product, int>().GetAllAsync(productSpecification);
            if(products is null || !products.Any())
                throw new Exception("GetAllProducts -> No Products found");

            ProductWithTotalCountSpecification ProductCountSpecification = new ProductWithTotalCountSpecification(productSpecificationParams);
            int totalCount = await unitOfWork.GetRepository<Product, int>().CountAsync(ProductCountSpecification);

            return new PaginatedResult<ProductDto>(
                totalItemsCount : totalCount,
                pageIndex : productSpecificationParams.PageIndex,
                pageSize: productSpecificationParams.PageSize,
                itemsList : mapper.Map<IEnumerable<ProductDto>>(products)
            );
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
