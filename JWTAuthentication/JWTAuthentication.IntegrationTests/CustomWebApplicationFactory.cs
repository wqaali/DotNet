using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit;

namespace JWTAuthentication.IntegrationTests
{
    /// <summary>
    /// Custom WebApplicationFactory for integration testing
    /// </summary>
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        public HttpClient HttpClient { get; private set; } = null!;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration((context, config) =>
            {
                // Add test configuration
                config.AddJsonFile("appsettings.Test.json", optional: false, reloadOnChange: true);
            });

            builder.ConfigureServices(services =>
            {
                // Remove any existing logging providers and add console logging for tests
                services.AddLogging(builder => builder.AddConsole().SetMinimumLevel(LogLevel.Warning));
            });

            builder.UseEnvironment("Development");
        }

        public async Task InitializeAsync()
        {
            HttpClient = CreateClient();
            await Task.CompletedTask;
        }

        public new async Task DisposeAsync()
        {
            HttpClient?.Dispose();
            await base.DisposeAsync();
        }
    }
}
