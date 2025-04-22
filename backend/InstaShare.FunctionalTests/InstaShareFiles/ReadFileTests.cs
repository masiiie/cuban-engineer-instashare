using Xunit;
using System.Text;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;

using InstaShare.FunctionalTests.Common;

namespace InstaShare.FunctionalTests.InstaShareFiles;

public class ReadFileTests : BaseFunctionalTest
{
    public ReadFileTests(FunctionalTestWebAppFactory factory) : base(factory){}

    [Fact]
    public async Task GetAllFiles_ReturnsOkResult_WithFileItems()
    {
        // Act
        var response = await httpclient.GetAsync("/files");

        // Assert
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        Assert.Contains("testfile.txt", responseString); 
    }

    [Fact]
    public async Task ReadFile_ReturnsOkResult()
    {
        // Arrange
        var fileId = await TestUtilities.GetExistingFileIdAsync(_dbContext);
        
        // Act
        var response = await httpclient.GetAsync($"/files/{fileId}");

        // Assert
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        Assert.Contains("testfile.txt", responseString);
    }
}