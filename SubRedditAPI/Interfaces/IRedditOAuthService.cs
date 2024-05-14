namespace SubRedditAPI.Interfaces
{
    public interface IRedditOAuthService
    {
        Task<string> GetUserAuthorizationTokenAsync();
        Task<string> GetAccessTokenAsync();
        Task<string> GetRefreshTokenAsync();
    }
}
