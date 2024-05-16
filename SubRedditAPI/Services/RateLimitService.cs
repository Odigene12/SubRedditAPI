using Microsoft.Extensions.Caching.Memory;
using SubRedditAPI.Interfaces;

namespace SubRedditAPI.Services
{
    public class RateLimitService : IRateLimitService
    {
        private readonly IMemoryCache _memoryCache;

        public RateLimitService(IMemoryCache memoryCache) => _memoryCache = memoryCache;

        public bool IsRequestAtRateLimit(string apiBeingCalled, double rateLimitRemaining)
        {
            string cacheKey = $"{apiBeingCalled}_RateLimiter";
            int numberOfcalls = _memoryCache.Get<int?>(cacheKey) ?? 0;

            // Calls are made every minute
            if (numberOfcalls == rateLimitRemaining)
            {
                return true;
            }

            _memoryCache.Set(cacheKey, numberOfcalls++);
            return false;
        }

        public object? ReturnCachedData(string cacheKey) => _memoryCache.Get<object?>(cacheKey);
    }
}
