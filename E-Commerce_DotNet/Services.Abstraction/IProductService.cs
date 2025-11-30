using Shared;
using Shared.ProductDtos;

namespace Services.Abstraction
{
    public interface IProductService
    {
        public Task<PaginatedResult<ProductDto>> GetAllProducts(ProductSpecificationParams productSpecificationParams);
        public Task<ProductDto> GetProductById(int id);
    }
}
