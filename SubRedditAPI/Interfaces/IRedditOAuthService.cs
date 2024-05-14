namespace SubRedditAPI.Interfaces
{
    public interface IRedditOAuthService
    {
        Task<string> GetUserAuthorizationTokenAsync();
    }
}
