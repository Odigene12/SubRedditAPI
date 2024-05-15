using Newtonsoft.Json;

namespace SubRedditAPI.Models
{
    public class RedditTopPostResponse
    {
        [JsonProperty("data")]
        public RedditResponse Data { get; set; } = new();
    }
}
