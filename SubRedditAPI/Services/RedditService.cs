using SubRedditAPI.Interfaces;
using SubRedditAPI.Models;

namespace SubRedditAPI.Services
{
    public class RedditService: IRedditService
    {
        private readonly IRedditRepository _redditRepository;

        public RedditService(IRedditRepository redditRepository)
        {
            _redditRepository = redditRepository;
        }

        public async Task<List<RedditPostData>> GetPostWithMostUpVotes()
        {
            return await _redditRepository.GetPostWithMostUpVotes();
        }

        public async Task<Dictionary<string, int>> GetUsersWithMostPosts()
        {
            return await _redditRepository.GetUsersWithMostPosts();
        }
    }
}
