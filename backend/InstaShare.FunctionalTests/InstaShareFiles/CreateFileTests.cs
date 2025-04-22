using Xunit;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using InstaShare.FunctionalTests.Common;

namespace InstaShare.FunctionalTests.InstaShareFiles;

public class CreateFileTests : BaseFunctionalTest
{
    public CreateFileTests(FunctionalTestWebAppFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task CreateFile_ReturnsOkResult()
    {
        // Arrange
        var createFileDto = new
        {
            FileName = "testfile.txt",
            FileSize = 1024,
            FileType = "text/plain"
        };
        var content = new StringContent(JsonConvert.SerializeObject(createFileDto), Encoding.UTF8, "application/json");

        // Act
        var response = await httpclient.PostAsync("/files", content);

        // Assert
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        Assert.Contains("testfile.txt", responseString);
    }
}   

