using Newtonsoft.Json;
using SubRedditAPI.Interfaces;
using SubRedditAPI.Models;
using System.Net.Http.Headers;
using Serilog;

namespace SubRedditAPI.Repositories
{
    public class RedditRepository : IRedditRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private const string SubredditUrl = "https://oauth.reddit.com/r/gaming/top.json?sort=top&t=all&limit=100";

        public RedditRepository(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<HttpResponseMessage> GetSubRedditResponseAsync(string accessToken)
        {
            try
            {
                Log.Debug("Getting Gaming Subreddits");

                var client = _httpClientFactory.CreateClient("RedditClient");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                return await client.GetAsync(SubredditUrl);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error retrieving gaming subreddit from repository layer");
                throw;
            }
        }
    }
}
