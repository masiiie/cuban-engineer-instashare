using Xunit;
using System.Text;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;

using InstaShare.FunctionalTests.Common;

namespace InstaShare.FunctionalTests.InstaShareFiles;

public class DeleteFileTests : BaseFunctionalTest
{
    public DeleteFileTests(FunctionalTestWebAppFactory factory) : base(factory) {}

    [Fact]
    public async Task DeleteFile_ReturnsOkResult()
    {
        // Arrange
        var fileId = TestUtilities.GetExistingFileIdAsync(_dbContext).Result;

        // Act
        var response = await httpclient.DeleteAsync($"/files/{fileId}");

        // Assert
        response.EnsureSuccessStatusCode();
    }
}