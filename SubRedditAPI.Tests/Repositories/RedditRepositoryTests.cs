using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using SubRedditAPI.Interfaces;
using SubRedditAPI.Models;
using System.Net;

namespace SubRedditAPI.Repositories.Tests
{
    public class RedditRepositoryTests
    {
        private readonly Mock<IRedditOAuthService> _mockRedditOAuthService;
        private readonly Mock<IHttpClientFactory> _mockHttpClientFactory;
        private readonly Mock<IRateLimitService> _mockRateLimitService;

        public RedditRepositoryTests()
        {
            _mockRedditOAuthService = new Mock<IRedditOAuthService>();
            _mockHttpClientFactory = new Mock<IHttpClientFactory>();
            _mockRateLimitService = new Mock<IRateLimitService>();
        }

        [Fact]
        public async Task GetPostWithMostUpVotesAsync_ReturnsExpectedData()
        {
            // Arrange
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();

            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(GetMockRedditTopPostResponse())),
                    Headers =
                    {
                        { "X-Ratelimit-Remaining", "100" }
                    }
                });

            var mockHttpClient = new HttpClient(mockHttpMessageHandler.Object);

            _mockHttpClientFactory.Setup(x => x.CreateClient("RedditClient")).Returns(mockHttpClient);

            _mockRedditOAuthService.Setup(x => x.GetUserAuthorizationTokenAsync()).ReturnsAsync(JsonConvert.SerializeObject(new TokenResponse { AccessToken = "mock_access_token", TokenType = "bearer" }));

            _mockRateLimitService.Setup(x => x.IsRequestAtRateLimit(It.IsAny<string>(), It.IsAny<int>())).Returns(false);

            var repository = new RedditRepository(_mockRedditOAuthService.Object, _mockHttpClientFactory.Object, _mockRateLimitService.Object);

            // Act
            var result = await repository.GetPostWithMostUpVotesAsync();

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
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(GetMockRedditTopPostResponse())),
                    Headers =
                    {
                        { "X-Ratelimit-Remaining", "100" }
                    }
                });

            var mockHttpClient = new HttpClient(mockHttpMessageHandler.Object);

            _mockHttpClientFactory.Setup(x => x.CreateClient("RedditClient")).Returns(mockHttpClient);

            _mockRedditOAuthService.Setup(x => x.GetUserAuthorizationTokenAsync()).ReturnsAsync(JsonConvert.SerializeObject(new TokenResponse { AccessToken = "mock_access_token", TokenType = "bearer" }));

            _mockRateLimitService.Setup(x => x.IsRequestAtRateLimit(It.IsAny<string>(), It.IsAny<int>())).Returns(false);

            var repository = new RedditRepository(_mockRedditOAuthService.Object, _mockHttpClientFactory.Object, _mockRateLimitService.Object);

            // Act
            var result = await repository.GetUsersWithMostPostsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal(2, result["user1"]);
            Assert.Equal(1, result["user2"]);
        }

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