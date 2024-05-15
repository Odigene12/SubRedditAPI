namespace SubRedditAPI.Interfaces
{
    public interface IRateLimitService
    {
        bool IsRequestAtRateLimit(string apiBeingCalled, int rateLimitRemaining);
        object? ReturnCachedData(string cacheKey);
    }
}
