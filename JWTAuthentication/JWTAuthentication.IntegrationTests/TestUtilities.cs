using System.Text;
using System.Text.Json;
using JWTAuthentication.Authentication;

namespace JWTAuthentication.IntegrationTests
{
    /// <summary>
    /// Utility class for integration test helpers
    /// </summary>
    public static class TestUtilities
    {
        /// <summary>
        /// Creates a JSON content for HTTP requests
        /// </summary>
        public static StringContent CreateJsonContent<T>(T obj)
        {
            var json = JsonSerializer.Serialize(obj);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        /// <summary>
        /// Creates a test user model
        /// </summary>
        public static UserModel CreateTestUser(string username = "testuser", string password = "testpass", string email = "test@example.com")
        {
            return new UserModel
            {
                UserName = username,
                Password = password,
                Email = email
            };
        }

        /// <summary>
        /// Creates a test auth body for refresh token requests
        /// </summary>
        public static AuthBody CreateTestAuthBody(string accessToken, string refreshToken)
        {
            return new AuthBody
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        /// <summary>
        /// Extracts JWT token from response content
        /// </summary>
        public static async Task<AuthBody?> ExtractAuthBodyFromResponse(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<AuthBody>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }

        /// <summary>
        /// Creates an HTTP request with JWT authorization header
        /// </summary>
        public static HttpRequestMessage CreateAuthenticatedRequest(HttpMethod method, string url, string token)
        {
            var request = new HttpRequestMessage(method, url);
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            return request;
        }
    }
}


