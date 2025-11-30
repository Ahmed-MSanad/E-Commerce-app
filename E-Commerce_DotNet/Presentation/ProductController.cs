using Microsoft.AspNetCore.Mvc;
using Services.Abstraction;
using Shared;
using Shared.ProductDtos;
namespace Presentation
{
    [Route("[controller]/[action]")]
    //[Authorize]
    public class ProductController(IServiceManager serviceManager) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<PaginatedResult<ProductDto>>> GetProducts([FromQuery] ProductSpecificationParams productSpecificationParams)
            => Ok(await serviceManager.ProductService.GetAllProducts(productSpecificationParams));

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProductDetails(int id)
            => Ok(await serviceManager.ProductService.GetProductById(id));
    }
}
