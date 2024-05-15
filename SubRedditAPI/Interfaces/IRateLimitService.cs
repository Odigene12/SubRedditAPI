namespace SubRedditAPI.Interfaces
{
    public interface IRateLimitService
    {
        bool IsRequestAtRateLimit(string apiBeingCalled, int rateLimitRemaining);
    }
}
