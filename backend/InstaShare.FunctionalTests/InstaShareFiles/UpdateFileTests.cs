using Xunit;
using System.Text;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;

using InstaShare.FunctionalTests.Common;

namespace InstaShare.FunctionalTests.InstaShareFiles;

public class UpdateFileTests : BaseFunctionalTest
{
    public UpdateFileTests(FunctionalTestWebAppFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task UpdateFile_ReturnsOkResult()
    {
        // Arrange
        var fileId = await TestUtilities.GetExistingFileIdAsync(_dbContext);
        var updatedFile = new { Name = "Updated File", Size = 2048 };
        var content = new StringContent(JsonConvert.SerializeObject(updatedFile), Encoding.UTF8, "application/json");

        // Act
        var response = await httpclient.PutAsync($"/files/{fileId}", content);

        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299
    }
}