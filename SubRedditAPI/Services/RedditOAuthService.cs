using SubRedditAPI.Configurations;
using SubRedditAPI.Interfaces;

namespace SubRedditAPI.Services
{
    public class RedditOAuthService: IRedditOAuthService
    {
        private readonly RedditAPIConfig _redditAPIConfig;
        private readonly IHttpClientFactory _httpClientFactory;

        public RedditOAuthService(RedditAPIConfig redditAPIConfig, IHttpClientFactory httpClientFactory)
        {
            _redditAPIConfig = redditAPIConfig;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string> GetUserAuthorizationTokenAsync()
        {
            var client = _httpClientFactory.CreateClient("RedditClient");
            var request = new HttpRequestMessage(HttpMethod.Post, "api/v1/access_token");

            return "";
        }

        public async Task<string> GetAccessTokenAsync()
        {
            var client = _httpClientFactory.CreateClient("RedditClient");
            var request = new HttpRequestMessage(HttpMethod.Post, "api/v1/access_token");


            return "";
        }

        public async Task<string> GetRefreshTokenAsync()
        {
            var client = _httpClientFactory.CreateClient("RedditClient");
            var request = new HttpRequestMessage(HttpMethod.Post, "api/v1/access_token");

            return "";
        }
    }
}
