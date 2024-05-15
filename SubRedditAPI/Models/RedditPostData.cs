using Newtonsoft.Json;

namespace SubRedditAPI.Models
{
    public class RedditPostData
    {
        [JsonProperty("data")]
        public Post? PostData { get; set; } = new();
    }
}
