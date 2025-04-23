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
        if (PostgreSqlContainer != null)
        {
            try
            {
                await PostgreSqlContainer.StopAsync();
            }
            catch (Exception ex)
            {
                //_debugger.Write($"Failed to stop PostgreSqlContainer: {ex.Message}");
                Console.WriteLine($"Failed to stop PostgreSqlContainer: {ex.Message}");
            }
        }
        else
        {
            //_debugger.Write("PostgreSqlContainer was never initialized");
        }
    }
}