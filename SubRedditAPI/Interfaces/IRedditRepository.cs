using SubRedditAPI.Models;

namespace SubRedditAPI.Interfaces
{
    public interface IRedditRepository
    {
        Task<HttpResponseMessage> GetSubRedditAsync();

        Task<List<RedditPostData>> GetPostWithMostUpVotes();

        Task<Dictionary<string, int>> GetUsersWithMostPosts();
    }
}
