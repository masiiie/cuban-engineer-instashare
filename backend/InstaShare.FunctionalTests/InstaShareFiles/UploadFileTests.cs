using Xunit;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using InstaShare.FunctionalTests.Common;


namespace InstaShare.FunctionalTests.InstaShareFiles;

public class UploadFileTests : BaseFunctionalTest
{
    public UploadFileTests(FunctionalTestWebAppFactory factory) : base(factory)
    {
    }

    [Fact]
    //[Fact(Skip = "This test is skipped for now.")]
    public async Task UploadFile_ReturnsOkResult()
    {
        // Arrange
        var fileId = await TestUtilities.GetExistingFileIdAsync(_dbContext);
        var filePath = "testfile.txt"; // Path to the file you want to upload

        using var content = new MultipartFormDataContent();
        using var fileStream = System.IO.File.OpenRead(filePath);
        content.Add(new StreamContent(fileStream), "file", filePath);

        // Act
        var response = await httpclient.PostAsync($"/files/{fileId}/upload", content);

        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299
    }