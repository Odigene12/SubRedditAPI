using SubRedditAPI.Models;

namespace SubRedditAPI.Interfaces
{
    public interface IRedditRepository
    {
        Task<HttpResponseMessage?> GetSubRedditResponseAsync();

        Task<List<RedditPostData?>?> GetPostWithMostUpVotesAsync();

        Task<Dictionary<string, int>?> GetUsersWithMostPostsAsync();
    }
}
