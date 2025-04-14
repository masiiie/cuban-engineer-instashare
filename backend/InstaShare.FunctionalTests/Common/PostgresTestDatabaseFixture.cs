using System;
using System.Threading.Tasks;
using Testcontainers.PostgreSql;
using Xunit;

public class PostgresTestDatabaseFixture : IAsyncLifetime
{
    public PostgreSqlContainer PostgreSqlContainer { get; private set; }

    public async Task InitializeAsync()
    {        
        PostgreSqlContainer = new PostgreSqlBuilder()
        .WithImage("postgis/postgis:latest")
        .WithDatabase("instashare")
        .Build();

        await PostgreSqlContainer.StartAsync();
    }

    public async Task DisposeAsync()
    {
        await PostgreSqlContainer.StopAsync();
    }
}