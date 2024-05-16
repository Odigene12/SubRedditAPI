using SubRedditAPI.Models;

namespace SubRedditAPI.Interfaces
{
    public interface IRedditService
    {
        Task<List<RedditPostData?>?> GetPostWithMostUpVotesAsync();
        Task<Dictionary<string,int>?> GetUsersWithMostPostsAsync();
    }
}
