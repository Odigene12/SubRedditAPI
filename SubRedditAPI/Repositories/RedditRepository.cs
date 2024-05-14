using Newtonsoft.Json;
using SubRedditAPI.Interfaces;
using SubRedditAPI.Models;
using System.Net.Http.Headers;

namespace SubRedditAPI.Repositories
{
    public class RedditRepository: IRedditRepository
    {
        private readonly IRedditOAuthService _redditOAuthService;
        private readonly IHttpClientFactory _httpClientFactory;

        // The URL to get the top posts from the anime subreddit for the month in JSON format
        private const string SubredditUrl = "https://oauth.reddit.com/r/anime/top.json?sort=top&t=year&limit=100";
        public RedditRepository(IRedditOAuthService redditOAuthService, IHttpClientFactory httpClientFactory) 
        {
            _redditOAuthService = redditOAuthService;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<List<RedditPostData>> GetPostWithMostUpVotes()
        {
            var subRedditResponse = await GetSubRedditAsync();
            if (subRedditResponse.IsSuccessStatusCode)
            {
                var content = await subRedditResponse.Content.ReadAsStringAsync();
                var topPostResponse = JsonConvert.DeserializeObject<RedditTopPostResponse>(content);
                if (topPostResponse?.Data?.RedditPostDataList?.Count > 0)
                {
                    var mostUpvotedPosts = topPostResponse?.Data?.RedditPostDataList?.OrderByDescending(x => x.PostData?.Upvotes)?.ToList();
                    return mostUpvotedPosts ?? new List<RedditPostData>();
                }
            }
            return new List<RedditPostData>();
        }

        public async Task<Dictionary<string,int>> GetUsersWithMostPosts()
        {
            var subRedditResponse = await GetSubRedditAsync();
            var authorPosts = new Dictionary<string, int>();
            if (subRedditResponse.IsSuccessStatusCode)
            {
                var content = await subRedditResponse.Content.ReadAsStringAsync();
                var topPostResponse = JsonConvert.DeserializeObject<RedditTopPostResponse>(content);
                if (topPostResponse?.Data?.RedditPostDataList?.Count > 0)
                {
                    var topPosts = topPostResponse?.Data?.RedditPostDataList?.OrderByDescending(x => x.PostData?.Upvotes)?.ToList();
                    foreach (var post in topPosts)
                    {
                        if (authorPosts.ContainsKey(post.PostData.Author))
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
            return authorPosts.OrderByDescending(author => author.Value).ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        public async Task<HttpResponseMessage> GetSubRedditAsync()
        {
            //get Authorization Token
            var authResponse = await _redditOAuthService.GetUserAuthorizationTokenAsync();
            var accessToken = JsonConvert.DeserializeObject<TokenResponse>(authResponse);


            _httpClientFactory.CreateClient("RedditClient").DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(accessToken.TokenType, accessToken.AccessToken);
            var client = _httpClientFactory.CreateClient("RedditClient");
            var response = await client.GetAsync($"{SubredditUrl}");

            return response;
        }
    }
}
