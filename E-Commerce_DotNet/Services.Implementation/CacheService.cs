using System.Text.Json;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc;
using Domain.Contracts;
using Services.Abstraction;

namespace Services.Implementation
{
    public class CacheService(ICacheRepository cacheRepository, IOptions<JsonOptions> jsonOptions) : ICacheService
    {
        private readonly JsonSerializerOptions _jsonOptions = jsonOptions.Value.JsonSerializerOptions;

        public async Task<string?> GetAsync(string cacheKey)
            => await cacheRepository.GetAsync(cacheKey);

        public async Task SetAsync(string cacheKey, object value, TimeSpan timeToLive)
            => await cacheRepository.SetAsync(cacheKey, JsonSerializer.Serialize(value, _jsonOptions), timeToLive);
    }
}