using SubRedditAPI.Models;

namespace SubRedditAPI.Interfaces
{
    public interface IRedditService
    {
        Task<List<RedditPostData>> GetPostWithMostUpVotes();
        Task<Dictionary<string,int>> GetUsersWithMostPosts();
    }
}
