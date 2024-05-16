using Newtonsoft.Json;
using SubRedditAPI.Interfaces;
using SubRedditAPI.Models;
using Serilog;

namespace SubRedditAPI.Services
{
    public class RedditService : IRedditService
    {
        private readonly IRedditOAuthService _redditOAuthService;
        private readonly IRedditRepository _redditRepository;
        private readonly IRateLimitService _rateLimitService;
        private const string CacheKey = "GetSubRedditResponseAsync";

        public RedditService(
            IRedditOAuthService redditOAuthService,
            IRedditRepository redditRepository,
            IRateLimitService rateLimitService)
        {
            _redditOAuthService = redditOAuthService;
            _redditRepository = redditRepository;
            _rateLimitService = rateLimitService;
        }

        public async Task<List<RedditPostData?>?> GetPostWithMostUpVotesAsync()
        {
            try
            {
                Log.Debug("Getting Posts with Most Upvotes");

                var response = await GetAuthorizedSubRedditResponseAsync();
                var cachedData = HandleRateLimit(response, CacheKey);

                if (cachedData != null)
                {
                    return (List<RedditPostData?>)cachedData;
                }

                if (response.IsSuccessStatusCode)
                {
                    var redditPostDataList = await ExtractPostDataAsync(response);
                    if (redditPostDataList != null)
                    {
                        Log.Debug($"Successfully retrieved {redditPostDataList.Count} Posts");
                        return redditPostDataList.OrderByDescending(x => x?.PostData?.Upvotes).ToList();
                    }
                }

                return new List<RedditPostData?>();

            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error retrieving posts with most upvotes from service layer");
                throw;
            }
        }

        public async Task<Dictionary<string, int>?> GetUsersWithMostPostsAsync()
        {
            try
            {
                Log.Debug("Getting Users with Most Posts");

                var response = await GetAuthorizedSubRedditResponseAsync();
                var cachedData = HandleRateLimit(response, CacheKey);

                if (cachedData != null)
                {
                    return (Dictionary<string, int>)cachedData;
                }

                if (response.IsSuccessStatusCode)
                {
                    var redditPostDataList = await ExtractPostDataAsync(response);
                    if (redditPostDataList != null)
                    {
                        Log.Debug($"Successfully retrieved {redditPostDataList.Count} Gaming Subreddits");
                        return CountPostsByAuthor(redditPostDataList);
                    }
                }

                return new Dictionary<string, int>();

            }
            catch (Exception ex)
            {
                Log.Error(ex , "Error retrieving users with most posts from service layer");
                throw;
            }
        }

        private async Task<HttpResponseMessage> GetAuthorizedSubRedditResponseAsync()
        {
            var authResponse = await _redditOAuthService.GetUserAuthorizationTokenAsync();
            var accessToken = JsonConvert.DeserializeObject<TokenResponse>(authResponse);
            return await _redditRepository.GetSubRedditResponseAsync(accessToken.AccessToken);
        }

        private object HandleRateLimit(HttpResponseMessage response, string apiBeingCalled)
        {
            double remainingCalls = double.Parse(response.Headers.GetValues("X-Ratelimit-Remaining").First());
            bool isRequestLimitReached = _rateLimitService.IsRequestAtRateLimit(apiBeingCalled, remainingCalls);

            if (isRequestLimitReached)
            {
                Log.Warning($"Request Limit has been reached for {apiBeingCalled} call");
                Log.Information($"Returning cached data for {apiBeingCalled} call");
                return _rateLimitService.ReturnCachedData(CacheKey);
            }

            return null;
        }

        private async Task<List<RedditPostData?>> ExtractPostDataAsync(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();
            var topPostResponse = JsonConvert.DeserializeObject<RedditTopPostResponse>(content);
            return topPostResponse?.Data?.RedditPostDataList ?? new List<RedditPostData?>();
        }

        private Dictionary<string, int> CountPostsByAuthor(List<RedditPostData?> redditPostDataList)
        {
            var authorPosts = new Dictionary<string, int>();
            foreach (var post in redditPostDataList)
            {
                var author = post?.PostData?.Author;
                if (author != null)
                {
                    if (authorPosts.ContainsKey(author))
                    {
                        authorPosts[author]++;
                    }
                    else
                    {
                        authorPosts[author] = 1;
                    }
                }
            }

            return authorPosts.OrderByDescending(author => author.Value).ToDictionary(pair => pair.Key, pair => pair.Value);
        }
    }
}
