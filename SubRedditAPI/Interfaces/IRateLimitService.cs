namespace SubRedditAPI.Interfaces
{
    public interface IRateLimitService
    {
        bool IsRequestAtRateLimit(string apiBeingCalled, double rateLimitRemaining);
        object? ReturnCachedData(string cacheKey);
    }
}
