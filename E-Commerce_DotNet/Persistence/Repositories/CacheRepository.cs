using Domain.Contracts;
using StackExchange.Redis;

namespace Persistence.Repositories
{
    public class CacheRepository(IConnectionMultiplexer connectionMultiplexer) : ICacheRepository
    {
        private readonly IDatabase _database = connectionMultiplexer.GetDatabase();
        public async Task<string?> GetAsync(string cacheKey) 
            => await _database.StringGetAsync(cacheKey);

        public async Task SetAsync(string cacheKey, string value, TimeSpan timeToLive) 
            => await _database.StringSetAsync(cacheKey, value, timeToLive);
    }
}
