using Microsoft.AspNetCore.Mvc;
using Presentation.Attributes;
using Services.Abstraction;
using Shared;
using Shared.ProductDtos;
namespace Presentation
{
    //[Authorize]
    public class ProductController(IServiceManager serviceManager) : ApiController
    {
        [HttpGet]
        [RedisCache(60)]
        public async Task<ActionResult<PaginatedResult<ProductDto>>> GetProducts([FromQuery] ProductSpecificationParams productSpecificationParams)
            => Ok(await serviceManager.ProductService.GetAllProducts(productSpecificationParams));

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProductDetails(int id)
            => Ok(await serviceManager.ProductService.GetProductById(id));
    }
}
