using FluentAssertions;
using System.Net;
using Xunit;

namespace JWTAuthentication.IntegrationTests
{
    /// <summary>
    /// Integration tests for API endpoints and HTTP responses
    /// </summary>
    public class ApiIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;
        private readonly HttpClient _client;

        public ApiIntegrationTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            _client = factory.HttpClient;
        }

        [Fact]
        public async Task SwaggerEndpoint_ShouldBeAvailable_InDevelopmentMode()
        {
            // Act
            var response = await _client.GetAsync("/swagger/v1/swagger.json");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await response.Content.ReadAsStringAsync();
            content.Should().Contain("JWTAuthetication");
        }

        [Fact]
        public async Task SwaggerUI_ShouldBeAvailable_InDevelopmentMode()
        {
            // Act
            var response = await _client.GetAsync("/swagger/index.html");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await response.Content.ReadAsStringAsync();
            content.Should().Contain("swagger");
        }

        [Fact]
        public async Task AuthenticationController_ShouldReturnCorrectContentType_ForJsonResponses()
        {
            // Arrange
            var user = TestUtilities.CreateTestUser();
            var jsonContent = TestUtilities.CreateJsonContent(user);

            // Act
            var response = await _client.PostAsync("/Authentication/login", jsonContent);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Content.Headers.ContentType?.MediaType.Should().Be("application/json");
        }

        //[Fact]
        //public async Task AuthenticationController_ShouldHandleCORS_WhenConfigured()
        //{
        //    // Act
        //    var request = new HttpRequestMessage(HttpMethod.Options, "/Authentication/login");
        //    request.Headers.Add("Origin", "https://example.com");
        //    request.Headers.Add("Access-Control-Request-Method", "POST");

        //    var response = await _client.SendAsync(request);

        //    // Assert
        //    // Note: CORS is not explicitly configured in the current setup,
        //    // but this test demonstrates how to test CORS when it's added
        //    response.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.NoContent);
        //}

        [Fact]
        public async Task AuthenticationController_ShouldReturnNotFound_ForInvalidEndpoint()
        {
            // Act
            var response = await _client.GetAsync("/Authentication/NonExistentEndpoint");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task AuthenticationController_ShouldHandleMethodNotAllowed_ForWrongHttpMethod()
        {
            // Act
            var response = await _client.GetAsync("/Authentication/login");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.MethodNotAllowed);
        }

        [Fact]
        public async Task AuthenticationController_ShouldReturnBadRequest_ForInvalidModel()
        {
            // Arrange
            var invalidUser = new { InvalidProperty = "test" };
            var jsonContent = TestUtilities.CreateJsonContent(invalidUser);

            // Act
            var response = await _client.PostAsync("/Authentication/login", jsonContent);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task AuthenticationController_ShouldReturnCorrectHeaders_ForSuccessfulRequests()
        {
            // Arrange
            var user = TestUtilities.CreateTestUser();
            var jsonContent = TestUtilities.CreateJsonContent(user);

            // Act
            var response = await _client.PostAsync("/Authentication/login", jsonContent);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            //response.Headers.Should().ContainKey("Date");
            response.Content.Headers.Should().ContainKey("Content-Type");
        }
    }
}
