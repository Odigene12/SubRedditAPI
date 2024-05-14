using Newtonsoft.Json;

namespace SubRedditAPI.Models
{
    public class Post
    {
        [JsonProperty("title")]
        public string Title { get; set; } = string.Empty;
        [JsonProperty("ups")]
        public int Upvotes { get; set; }

        [JsonProperty("author")]
        public string Author { get; set; } = string.Empty;

        [JsonProperty("num_comments")]
        public int NumberOfComments { get; set; }

        [JsonProperty("subreddit_subscribers")]
        public int NumberOfSubscribersForThisSubReddit { get; set; }

    }
}
