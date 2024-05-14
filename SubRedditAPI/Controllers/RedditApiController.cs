using Microsoft.AspNetCore.Mvc;
using SubRedditAPI.Interfaces;
using SubRedditAPI.Models;
using SubRedditAPI.Repositories;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SubRedditAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RedditApiController : ControllerBase
    {
        private readonly IRedditRepository _redditRepository;

        public RedditApiController(IRedditRepository redditRepository)
        {
            _redditRepository = redditRepository;
        }

        // GET: api/<RedditApiController>
        [HttpGet]
        public async Task<string> GetRedditStats()
        {
            return await _redditRepository.GetSubRedditAsync();
        }
    }
}
