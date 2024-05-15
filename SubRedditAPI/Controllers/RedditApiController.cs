using Microsoft.AspNetCore.Mvc;
using SubRedditAPI.Interfaces;
using SubRedditAPI.Models;
using Serilog;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SubRedditAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RedditApiController : ControllerBase
    {
        private readonly IRedditService _redditService;

        public RedditApiController(IRedditService redditService) => _redditService = redditService;

        // GET: api/<RedditApiController>
        [HttpGet]
        [Route("GetPostWithMostUpVotes")]
        public async Task<List<RedditPostData?>?> GetPostWithMostUpVotes()
        {
            Log.Information("Received GET request to GetPostWithMostUpVotes");
            try
            {
                var postWithMostUpVotes = await _redditService.GetPostWithMostUpVotes();
                return postWithMostUpVotes ?? new List<RedditPostData?>();
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "Request failed for GetPostWithMostUpVotes");
                throw;
            }
        }

        [HttpGet]
        [Route("GetUsersWithMostPosts")]
        public async Task<Dictionary<string, int>> GetUsersWithMostPosts()
        {
            Log.Information("Received GET request to GetUsersWithMostPosts");
            try
            {
                return await _redditService.GetUsersWithMostPosts();
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "Request failed for GetUsersWithMostPosts");
                throw;
            }
        }
    }
}
