using Xunit;
using Microsoft.Extensions.DependencyInjection;
using InstaShare.Infrastructure.Persistence;
using System.Net.Http;

namespace InstaShare.FunctionalTests.Common;

[Collection("PostgresTestDatabase")]
public class BaseFunctionalTest : IClassFixture<FunctionalTestWebAppFactory>
{
    protected readonly HttpClient httpclient;
    protected readonly ApplicationDbContext _dbContext;

    public BaseFunctionalTest(FunctionalTestWebAppFactory factory)
    {        
        httpclient = factory.CreateClient();
        var scope = factory.Services.CreateScope();
        _dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    }
}
