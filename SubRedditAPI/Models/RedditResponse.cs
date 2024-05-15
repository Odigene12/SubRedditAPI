using Newtonsoft.Json;

namespace SubRedditAPI.Models
{
    public class RedditResponse
    {
        [JsonProperty("children")]
        public List<RedditPostData?> RedditPostDataList { get; set; } = new();
    }
}
