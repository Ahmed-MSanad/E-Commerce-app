using AutoMapper;
using Domain.Contracts;
using Domain.Models.Basket;
using Services.Abstraction;
using Shared.BasketDtos;

namespace Services.Implementation
{
    public class BasketService(IBasketRepository basketRepository, IMapper mapper) : IBasketService
    {
        public async Task<bool> DeleteBasketAsync(string id) =>
            await basketRepository.DeleteBasketAsync(id);

        public async Task<BasketDto> GetBasketAsync(string id)
        {
            CustomerBasket basket = await basketRepository.GetBasketAsync(id);
            if (basket == null) throw new Exception($"The basket with id: {id} is not found");
            return mapper.Map<BasketDto>(basket);
        }

        public async Task<BasketDto> UpdateBasketAsync(BasketDto basket)
        {
            CustomerBasket customerBasket = mapper.Map<CustomerBasket>(basket);
            customerBasket = await basketRepository.UpdateBasketAsync(customerBasket);
            if(customerBasket is null) throw new Exception("Can't Update The basket now!!");
            return mapper.Map<BasketDto>(customerBasket);
        }
    }
}
