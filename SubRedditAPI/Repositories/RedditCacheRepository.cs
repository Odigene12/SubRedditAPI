using Microsoft.Extensions.Caching.Memory;
using SubRedditAPI.Interfaces;
using SubRedditAPI.Models;

namespace SubRedditAPI.Repositories
{
    public class RedditCacheRepository : IRedditRepository
    {
        private readonly RedditRepository _decoratedRepo;
        private readonly IMemoryCache _memoryCache;
        private const string CacheKey = "GetSubRedditResponseAsync";

        // decorate the RedditRepository with a cache layer for the Reddit API
        public RedditCacheRepository(RedditRepository decoratedRepo, IMemoryCache memoryCache)
        {

            _memoryCache = memoryCache;
            _decoratedRepo = decoratedRepo;
        } 

        public async Task<List<RedditPostData?>?> GetPostWithMostUpVotesAsync()
        {
            return await _memoryCache.GetOrCreateAsync(
                CacheKey, 
                async cacheEntry =>
                {
                    cacheEntry.SetAbsoluteExpiration(TimeSpan.FromSeconds(20));

                    return await _decoratedRepo.GetPostWithMostUpVotesAsync();
                });

        }

        public async Task<Dictionary<string, int>?> GetUsersWithMostPostsAsync()
        {
            return await _memoryCache.GetOrCreateAsync(
                CacheKey,
                async cacheEntry =>
                {
                    cacheEntry.SetAbsoluteExpiration(TimeSpan.FromSeconds(20));

                    return await _decoratedRepo.GetUsersWithMostPostsAsync();
                });
        } 
    }
}
