using Microsoft.Extensions.Caching.Memory;
using SubRedditAPI.Interfaces;
using SubRedditAPI.Models;

namespace SubRedditAPI.Repositories
{
    public class RedditCacheRepository : IRedditRepository
    {
        private readonly RedditRepository _decoratedRepo;
        private readonly IMemoryCache _memoryCache;

        // decorate the RedditRepository with a cache layer for the Reddit API
        public RedditCacheRepository(RedditRepository decoratedRepo, IMemoryCache memoryCache)
        {

            _memoryCache = memoryCache;
            _decoratedRepo = decoratedRepo;
        } 

        public Task<List<RedditPostData?>?> GetPostWithMostUpVotesAsync()
        {
            string cacheKey = "TopRedditPostsForGaming";

            return _memoryCache.GetOrCreateAsync(
                cacheKey, 
                cacheEntry =>
                {
                    cacheEntry.SetAbsoluteExpiration(TimeSpan.FromSeconds(20));

                    return _decoratedRepo.GetPostWithMostUpVotesAsync();
                });

        }

        public Task<HttpResponseMessage?> GetSubRedditResponseAsync()
        {
            string cacheKey = "GamingSubreddit";

            return _memoryCache.GetOrCreateAsync(
                cacheKey,
                cacheEntry =>
                {
                    cacheEntry.SetAbsoluteExpiration(TimeSpan.FromSeconds(20));

                    return _decoratedRepo.GetSubRedditResponseAsync();
                });
        }

        public Task<Dictionary<string, int>?> GetUsersWithMostPostsAsync()
        {
            string cacheKey = "TopUsers";

            return _memoryCache.GetOrCreateAsync(
                cacheKey,
                cacheEntry =>
                {
                    cacheEntry.SetAbsoluteExpiration(TimeSpan.FromSeconds(20));

                    return _decoratedRepo.GetUsersWithMostPostsAsync();
                });
        } 
    }
}
