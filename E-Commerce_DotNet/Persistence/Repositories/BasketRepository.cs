using System.Text.Json;
using Domain.Contracts;
using Domain.Models.Basket;
using StackExchange.Redis;

namespace Persistence.Repositories
{
    public class BasketRepository(IConnectionMultiplexer connection) : IBasketRepository
    {
        private readonly IDatabase database = connection.GetDatabase();
        public async Task<CustomerBasket> GetBasketAsync(string id)
        {
            var basket = await database.StringGetAsync(id);
            if (basket.IsNullOrEmpty) return null;
            return JsonSerializer.Deserialize<CustomerBasket>(basket);
        }
        public async Task<bool> DeleteBasketAsync(string id) => await database.KeyDeleteAsync(id);
        public async Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket, TimeSpan? timeToLive = null)
        {
            string serializedBasket = JsonSerializer.Serialize(basket);
            bool isCreatedOrUpdated = await database.StringSetAsync(basket.Id, serializedBasket, timeToLive ?? TimeSpan.FromDays(10));
            return isCreatedOrUpdated ? await GetBasketAsync(basket.Id) : null;
        }
    }
}
