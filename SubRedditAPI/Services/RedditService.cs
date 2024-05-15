using SubRedditAPI.Interfaces;
using SubRedditAPI.Models;
using Serilog;

namespace SubRedditAPI.Services
{
    public class RedditService: IRedditService
    {
        private readonly IRedditRepository _redditRepository;

        public RedditService(IRedditRepository redditRepository) => _redditRepository = redditRepository;

        public async Task<List<RedditPostData?>?> GetPostWithMostUpVotes()
        {
            try
            {
                return await _redditRepository.GetPostWithMostUpVotesAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error retrieving Users With Most Posts from Service Layer");
                throw;
            }
        }

        public async Task<Dictionary<string, int>> GetUsersWithMostPosts()
        {
            try
            {
                return await _redditRepository.GetUsersWithMostPostsAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error retrieving Users With Most Posts from Service Layer");
                throw;
            }
        }
    }
}
