using SubRedditAPI.Models;

namespace SubRedditAPI.Interfaces
{
    public interface IRedditRepository
    {
        Task<HttpResponseMessage> GetSubRedditResponseAsync(string accessToken);
    }
}
