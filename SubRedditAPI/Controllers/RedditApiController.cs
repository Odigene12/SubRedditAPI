using Microsoft.AspNetCore.Mvc;
using SubRedditAPI.Interfaces;
using SubRedditAPI.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SubRedditAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RedditApiController : ControllerBase
    {
        private readonly IRedditService _redditService;

        public RedditApiController(IRedditService redditService)
        {
            _redditService = redditService;
        }

        // GET: api/<RedditApiController>
        [HttpGet]
        [Route("GetPostWithMostUpVotes")]
        public async Task<List<RedditPostData>> GetPostWithMostUpVotes()
        {
            return await _redditService.GetPostWithMostUpVotes();
        }

        [HttpGet]
        [Route("GetUsersWithMostPosts")]
        public async Task<Dictionary<string, int>> GetUsersWithMostPosts()
        {
            return await _redditService.GetUsersWithMostPosts();
        }
    }
}
