using Microsoft.AspNetCore.Mvc;
using Services.Abstraction;
using Shared.BasketDtos;

namespace Presentation
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class BasketController(IServiceManager serviceManager) : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<ActionResult<BasketDto>> Get(string id)
        {
            BasketDto basketDto = await serviceManager.BasketService.GetBasketAsync(id);
            if(basketDto is null)
                return NotFound();
            return Ok(basketDto);
        }
        [HttpPut]
        public async Task<ActionResult<BasketDto>> Update(BasketDto basket)
        {
            return Ok(await serviceManager.BasketService.UpdateBasketAsync(basket));
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete(string id)
        {
            await serviceManager.BasketService.DeleteBasketAsync(id);
            return NoContent();
        }
    }
}
