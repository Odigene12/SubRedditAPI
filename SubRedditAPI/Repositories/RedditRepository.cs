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
        public RedditRepository(IRedditOAuthService redditOAuthService, IHttpClientFactory httpClientFactory) 
        {
            _redditOAuthService = redditOAuthService;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string> GetSubRedditAsync()
        {
            //get Authorization Token
            var authResponse = await _redditOAuthService.GetUserAuthorizationTokenAsync();
            var accessToken = JsonConvert.DeserializeObject<TokenResponse>(authResponse);
            

           _httpClientFactory.CreateClient("RedditClient").DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(accessToken.TokenType, accessToken.AccessToken);
            var client = _httpClientFactory.CreateClient("RedditClient");
            var response = await client.GetAsync($"{client.BaseAddress}/me");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            return "";
        }
    }
}
