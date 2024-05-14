using SubRedditAPI.Configurations;
using SubRedditAPI.Interfaces;
using System.Net.Http.Headers;
using System.Text;

namespace SubRedditAPI.Services
{
    public class RedditOAuthService: IRedditOAuthService
    {
        private readonly RedditAPIConfig _redditAPIConfig;
        private readonly IHttpClientFactory _httpClientFactory;
        private const string REDIRECT_URI = "https://localhost:7181";

        public RedditOAuthService(RedditAPIConfig redditAPIConfig, IHttpClientFactory httpClientFactory)
        {
            _redditAPIConfig = redditAPIConfig;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string> GetUserAuthorizationTokenAsync()
        {
            var client = _httpClientFactory.CreateClient("RedditClient");
            var data = new Dictionary<string, string>()
            {
                ["grant_type"] = "password",
                ["username"] = _redditAPIConfig.Username,
                ["password"] = _redditAPIConfig.Password,
                ["response_type"] = "code",
                ["redirect_uri"] = REDIRECT_URI,

            };
            var authTokenRequestUrl = $"{client.BaseAddress}/access_token";
            var request = new HttpRequestMessage(HttpMethod.Post, authTokenRequestUrl);
            var credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_redditAPIConfig.ClientId}:{_redditAPIConfig.ClientSecret}"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
            request.Content = new FormUrlEncodedContent(data);

            try
            {
                var response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }

            return "";
        }
    }
}
