using DotNet.Testcontainers.Builders;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Data.Common;
using Testcontainers.PostgreSql;


using InstaShare.Infrastructure.Persistence;

namespace InstaShare.FunctionalTests.Common;

public class FunctionalTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgresTestDatabaseFixture _fixture;

    public FunctionalTestWebAppFactory(PostgresTestDatabaseFixture fixture)
    {
        _fixture = fixture;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((context, config) =>
        {
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new[]
                {
                    new KeyValuePair<string, string>("ConnectionStrings:DefaultConnection", _fixture.PostgreSqlContainer.GetConnectionString())
                })
                .Build();

            config.AddConfiguration(configuration);
        });

        builder.ConfigureServices(services =>
        {
            // Ensure the database is created
            var serviceProvider = services.BuildServiceProvider();
            using var scope = serviceProvider.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<ApplicationDbContext>();
            db.Database.EnsureCreated();

            services.AddLogging(config =>
            {
                config.AddConsole();
                config.AddDebug();
                config.SetMinimumLevel(LogLevel.Debug); // Set the minimum log level to Debug
            });
        });

        builder.UseSetting("detailedErrors", "true");
        builder.CaptureStartupErrors(true);
    }

    public new async Task DisposeAsync()
    {
    }

    public async Task InitializeAsync()
    {
    }
}
