using Microsoft.Extensions.Caching.Memory;
using Moq;
using SubRedditAPI.Interfaces;
using SubRedditAPI.Services;
using Xunit;

public class RateLimitServiceTests
{
    private readonly Mock<IMemoryCache> _memoryCacheMock;
    private readonly RateLimitService _rateLimitService;

    public RateLimitServiceTests()
    {
        _memoryCacheMock = new Mock<IMemoryCache>();
        _rateLimitService = new RateLimitService(_memoryCacheMock.Object);
    }

    [Fact]
    public void IsRequestAtRateLimit_ShouldReturnTrue_WhenRateLimitIsReached()
    {
        // Arrange
        string apiBeingCalled = "GetSubRedditResponseAsync";
        int rateLimitRemaining = 5;
        string cacheKey = $"{apiBeingCalled}_RateLimiter";

        var memoryCacheEntryMock = new Mock<ICacheEntry>();

        _memoryCacheMock.Setup(m => m.CreateEntry(It.IsAny<object>())).Returns(memoryCacheEntryMock.Object);

        _memoryCacheMock.Setup(m => m.TryGetValue(cacheKey, out It.Ref<object>.IsAny))
            .Returns(true)
            .Callback((object key, out object value) =>
            {
                value = rateLimitRemaining; // Mocking the cache value to be the same as rateLimitRemaining
            });

        // Act
        var result = _rateLimitService.IsRequestAtRateLimit(apiBeingCalled, rateLimitRemaining);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsRequestAtRateLimit_ShouldReturnFalse_WhenRateLimitIsNotReached()
    {
        // Arrange
        string apiBeingCalled = "GetSubRedditResponseAsync";
        int rateLimitRemaining = 5;
        string cacheKey = $"{apiBeingCalled}_RateLimiter";

        var memoryCacheEntryMock = new Mock<ICacheEntry>();

        _memoryCacheMock.Setup(m => m.CreateEntry(It.IsAny<object>())).Returns(memoryCacheEntryMock.Object);

        _memoryCacheMock.Setup(m => m.TryGetValue(cacheKey, out It.Ref<object>.IsAny))
            .Returns(false);

        // Act
        var result = _rateLimitService.IsRequestAtRateLimit(apiBeingCalled, rateLimitRemaining);

        // Assert
        Assert.False(result);
    }
}
