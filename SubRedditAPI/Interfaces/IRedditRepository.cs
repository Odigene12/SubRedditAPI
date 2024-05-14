using SubRedditAPI.Models;

namespace SubRedditAPI.Interfaces
{
    public interface IRedditRepository
    {
        Task<HttpResponseMessage> GetSubRedditResponseAsync();

        Task<List<RedditPostData>> GetPostWithMostUpVotes();

        Task<Dictionary<string, int>> GetUsersWithMostPosts();
    }
}
