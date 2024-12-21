using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
namespace Redis
{
    public interface IRedisCacheService
    {
        Task<T?> GetAsync<T>(string key);
        Task SetAsync<T>(string key, T value, TimeSpan expiration);
        Task RemoveAsync(string key);
    }
    public class RedisCacheService(IDistributedCache cache) : IRedisCacheService
    {
        public async Task<T?> GetAsync<T>(string key)
        {
            var cacheData = await cache.GetStringAsync(key);
            return cacheData is null ? default : JsonSerializer.Deserialize<T>(cacheData);
        }

        public async Task RemoveAsync(string key)
        {
           await cache.RemoveAsync(key);
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan expiration)
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration
            };
            var serializeData = JsonSerializer.Serialize(value);
            await cache.SetStringAsync(key,serializeData, options);
        }
        
    }
}
