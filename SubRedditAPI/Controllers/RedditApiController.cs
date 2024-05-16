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

        [HttpGet]
        [Route("GetPostWithMostUpVotes")]
        public async Task<ActionResult<List<RedditPostData?>?>> GetPostWithMostUpVotes()
        {
            Log.Information("Received GET request to GetPostWithMostUpVotes");
            try
            {
                var postWithMostUpVotes = await _redditService.GetPostWithMostUpVotesAsync();
                return Ok(postWithMostUpVotes);
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "Request failed for GetPostWithMostUpVotes");
                return StatusCode(500, new { message = "An error occurred while processing your request.", error = ex.Message });
            }
        }

        [HttpGet]
        [Route("GetUsersWithMostPosts")]
        public async Task<ActionResult<Dictionary<string, int>>> GetUsersWithMostPosts()
        {
            Log.Information("Received GET request to GetUsersWithMostPosts");
            try
            {
                var usersWithMostPosts = await _redditService.GetUsersWithMostPostsAsync();
                return Ok(usersWithMostPosts);
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "Request failed for GetUsersWithMostPosts");
                return StatusCode(500,new { message = "An error occurred while processing your request.", error = ex.Message }); 
            }
        }
    }
}
