using Xunit;

namespace Worsite.FunctionalTests.Common;

[Collection("PostgresTestDatabase")]
public class BaseFunctionalTest : IClassFixture<FunctionalTestWebAppFactory>
{
    public BaseFunctionalTest(FunctionalTestWebAppFactory factory)
    {        
        httpclient = factory.CreateClient();
    }

    protected HttpClient httpclient { get; init; }
}
