using Domain.Models.Basket;

namespace Domain.Contracts
{
    public interface IBasketRepository
    {
        Task<CustomerBasket> GetBasketAsync(string id);
        Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket, TimeSpan? timeToLive = null);
        Task<bool> DeleteBasketAsync(string id);
    }
}
