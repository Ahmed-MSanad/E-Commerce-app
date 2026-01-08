namespace Services.Abstraction
{
    public interface ICacheService
    {
        Task<string?> GetAsync(string cacheKey);
        Task SetAsync(string cacheKey, object value, TimeSpan timeToLive);
    }
}
