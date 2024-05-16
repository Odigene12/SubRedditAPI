using Microsoft.Extensions.Caching.Memory;
using SubRedditAPI.Interfaces;
using SubRedditAPI.Models;
using SubRedditAPI.Services;

namespace SubRedditAPI.Repositories
{
    public class RedditCacheRepository : IRedditService
    {
        private readonly RedditService _decoratedService;
        private readonly IMemoryCache _memoryCache;
        private const string CacheKey = "GetSubRedditResponseAsync";

        // decorate the RedditService with a cache layer for the Reddit API
        public RedditCacheRepository(RedditService decoratedService, IMemoryCache memoryCache)
        {

            _memoryCache = memoryCache;
            _decoratedService = decoratedService;
        } 

        public async Task<List<RedditPostData?>?> GetPostWithMostUpVotesAsync()
        {
            return await _memoryCache.GetOrCreateAsync(
                CacheKey, 
                async cacheEntry =>
                {
                    cacheEntry.SetAbsoluteExpiration(TimeSpan.FromSeconds(20));

                    return await _decoratedService.GetPostWithMostUpVotesAsync();
                });

        }

        public async Task<Dictionary<string, int>?> GetUsersWithMostPostsAsync()
        {
            return await _memoryCache.GetOrCreateAsync(
                CacheKey,
                async cacheEntry =>
                {
                    cacheEntry.SetAbsoluteExpiration(TimeSpan.FromSeconds(20));

                    return await _decoratedService.GetUsersWithMostPostsAsync();
                });
        } 
    }
}
