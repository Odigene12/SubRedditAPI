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
        private readonly Mock<IHttpClientFactory> _mockHttpClientFactory;

        public RedditRepositoryTests()
        {
            _mockHttpClientFactory = new Mock<IHttpClientFactory>();
        }

        [Fact]
        public async Task GetSubRedditResponseAsync_ReturnsExpectedData()
        {
            // Arrange
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();

            // Setup the mock response for the Reddit API call to return the expected data
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

            var repository = new RedditRepository(_mockHttpClientFactory.Object);

            // Act
            var result = await repository.GetSubRedditResponseAsync("mock_access_token");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
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
