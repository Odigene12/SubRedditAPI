using Newtonsoft.Json;
using SubRedditAPI.Interfaces;
using SubRedditAPI.Models;
using System.Net.Http.Headers;
using Serilog;

namespace SubRedditAPI.Repositories
{
    public class RedditRepository: IRedditRepository
    {
        private readonly IRedditOAuthService _redditOAuthService;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IRateLimitService _rateLimitService;

        // The URL to get the top posts from the anime subreddit for the month in JSON format
        private const string SubredditUrl = "https://oauth.reddit.com/r/gaming/top.json?sort=top&t=all&limit=100";
        public RedditRepository(
            IRedditOAuthService redditOAuthService, 
            IHttpClientFactory httpClientFactory,
            IRateLimitService rateLimitService) 
        {
            _redditOAuthService = redditOAuthService;
            _httpClientFactory = httpClientFactory;
            _rateLimitService = rateLimitService;
        }

        public async Task<List<RedditPostData?>?> GetPostWithMostUpVotesAsync()
        {
            try
            {
                Log.Debug("Getting Posts with Most Upvotes");

                var subRedditResponse = await GetSubRedditResponseAsync();

                await HandleRateLimit(subRedditResponse, "GetSubRedditResponseAsync");

                if (subRedditResponse.IsSuccessStatusCode)
                {
                    var content = await subRedditResponse.Content.ReadAsStringAsync();
                    var topPostResponse = JsonConvert.DeserializeObject<RedditTopPostResponse>(content);
                    List<RedditPostData?>? redditPostDataList = topPostResponse?.Data?.RedditPostDataList;
                    if (redditPostDataList?.Count > 0)
                    {
                        Log.Debug($"Successfully retrieved {redditPostDataList?.Count} Posts");

                        List<RedditPostData?>? mostUpvotedPosts = redditPostDataList?.OrderByDescending(x => x?.PostData?.Upvotes).ToList();
                        return mostUpvotedPosts ?? new List<RedditPostData?>();
                    }
                }
                return new List<RedditPostData?>();

            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error retrieving posts with most upvotes from repository layer");
                throw;
            }
        }

        public async Task<Dictionary<string, int>?> GetUsersWithMostPostsAsync()
        {
            try
            {
                Log.Debug("Getting Users with Most Upvotes");

                var subRedditResponse = await GetSubRedditResponseAsync();
                var authorPosts = new Dictionary<string, int>();

                await HandleRateLimit(subRedditResponse, "GetSubRedditResponseAsync");

                if (subRedditResponse.IsSuccessStatusCode)
                {
                    var content = await subRedditResponse.Content.ReadAsStringAsync();
                    var topPostResponse = JsonConvert.DeserializeObject<RedditTopPostResponse>(content);
                    if (topPostResponse?.Data?.RedditPostDataList?.Count > 0)
                    {
                        var topPosts = topPostResponse?.Data?.RedditPostDataList?.OrderByDescending(x => x?.PostData?.Upvotes)?.ToList();
                        Log.Debug($"Successfully retrieved {topPosts?.Count} Gaming Subreddits");

                        if (topPosts?.Count > 0)
                        {
                            foreach (var post in topPosts)
                            {
                                if (authorPosts.ContainsKey(post?.PostData?.Author))
                                {
                                    authorPosts[post.PostData.Author]++;
                                }
                                else
                                {
                                    authorPosts.Add(post.PostData.Author, 1);
                                }
                            }
                        }

                    }
                }
                return authorPosts.OrderByDescending(author => author.Value).ToDictionary(pair => pair.Key, pair => pair.Value);

            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error retrieving users with most Posts from repository layer");
                throw;
            }
        }

        public async Task<HttpResponseMessage?> GetSubRedditResponseAsync()
        {
            try
            {
                Log.Debug("Getting Gaming Subreddits");

                var authResponse = await _redditOAuthService.GetUserAuthorizationTokenAsync();
                var accessToken = JsonConvert.DeserializeObject<TokenResponse>(authResponse);


                _httpClientFactory.CreateClient("RedditClient").DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(accessToken?.TokenType, accessToken?.AccessToken);
                var client = _httpClientFactory.CreateClient("RedditClient");
                var response = await client.GetAsync($"{SubredditUrl}");

                return response;

            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error retrieving gaming subreddit from repository layer");
                throw;
            }
        }

        private async Task HandleRateLimit(HttpResponseMessage? subRedditResponse, string apiBeingCalled)
        {
            int remainingNumberOfCalls = int.Parse(subRedditResponse.Headers.GetValues("X-Ratelimit-Remaining").First());

            bool isRequestLimitReached = _rateLimitService.IsRequestAtRateLimit(apiBeingCalled, remainingNumberOfCalls);

            if (isRequestLimitReached)
            {
                Log.Warning($"Request Limit has been reached for {apiBeingCalled} call");
                Log.Debug("Delaying request by 1 Minute");
                await Task.Delay(1000);
            }
        }
    }
}
