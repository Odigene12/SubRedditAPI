using Moq;
using Newtonsoft.Json;
using SubRedditAPI.Interfaces;
using SubRedditAPI.Models;
using System.Net;

namespace SubRedditAPI.Services.Tests
{
    public class RedditServiceTests
    {
        private readonly Mock<IRedditOAuthService> _mockRedditOAuthService;
        private readonly Mock<IRedditRepository> _mockRedditRepository;
        private readonly Mock<IRateLimitService> _mockRateLimitService;

        public RedditServiceTests()
        {
            _mockRedditOAuthService = new Mock<IRedditOAuthService>();
            _mockRedditRepository = new Mock<IRedditRepository>();
            _mockRateLimitService = new Mock<IRateLimitService>();
        }

        [Fact]
        public async Task GetPostWithMostUpVotesAsync_ReturnsExpectedData()
        {
            // Arrange
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(GetMockRedditTopPostResponse())),
                Headers =
                {
                    { "X-Ratelimit-Remaining", "100" }
                }
            };

            _mockRedditOAuthService.Setup(x => x.GetUserAuthorizationTokenAsync()).ReturnsAsync(
                JsonConvert.SerializeObject(
                    new TokenResponse
                    {
                        AccessToken = "mock_access_token",
                        TokenType = "bearer"
                    }));

            _mockRedditRepository.Setup(x => x.GetSubRedditResponseAsync(It.IsAny<string>())).ReturnsAsync(response);
            _mockRateLimitService.Setup(x => x.IsRequestAtRateLimit(It.IsAny<string>(), It.IsAny<int>())).Returns(false);

            var service = new RedditService(_mockRedditOAuthService.Object, _mockRedditRepository.Object, _mockRateLimitService.Object);

            // Act
            var result = await service.GetPostWithMostUpVotesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            Assert.Equal(253524, result[0]?.PostData?.Upvotes);
            Assert.Equal(456, result[1]?.PostData?.Upvotes);
        }

        [Fact]
        public async Task GetUsersWithMostPostsAsync_ReturnsExpectedData()
        {
            // Arrange
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(GetMockRedditTopPostResponse())),
                Headers =
                {
                    { "X-Ratelimit-Remaining", "100" }
                }
            };

            _mockRedditOAuthService.Setup(x => x.GetUserAuthorizationTokenAsync()).ReturnsAsync(
                JsonConvert.SerializeObject(
                    new TokenResponse
                    {
                        AccessToken = "mock_access_token",
                        TokenType = "bearer"
                    }));

            _mockRedditRepository.Setup(x => x.GetSubRedditResponseAsync(It.IsAny<string>())).ReturnsAsync(response);
            _mockRateLimitService.Setup(x => x.IsRequestAtRateLimit(It.IsAny<string>(), It.IsAny<int>())).Returns(false);

            var service = new RedditService(_mockRedditOAuthService.Object, _mockRedditRepository.Object, _mockRateLimitService.Object);

            // Act
            var result = await service.GetUsersWithMostPostsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal(2, result["user1"]);
            Assert.Equal(1, result["user2"]);
        }

        // Helper method to return a mock RedditTopPostResponse object with expected data for testing
        private RedditTopPostResponse GetMockRedditTopPostResponse()
        {
            return new RedditTopPostResponse
            {
                Data = new RedditResponse
                {
                    RedditPostDataList = new List<RedditPostData?>
                    {
                        new RedditPostData
                        {
                            PostData = new Post
                            {
                                Upvotes = 253524,
                                Author = "user1"
                            }
                        },
                        new RedditPostData
                        {
                            PostData = new Post
                            {
                                Upvotes = 456,
                                Author = "user2"
                            }
                        },
                        new RedditPostData
                        {
                            PostData = new Post
                            {
                                Upvotes = 25,
                                Author = "user1"
                            }
                        }
                    }
                }
            };
        }
    }
}
