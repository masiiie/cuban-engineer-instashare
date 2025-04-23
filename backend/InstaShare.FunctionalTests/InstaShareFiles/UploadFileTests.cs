using Xunit;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using InstaShare.FunctionalTests.Common;
using InstaShare.Domain.Entities.Files;

namespace InstaShare.FunctionalTests.InstaShareFiles;

public class UploadFileTests : BaseFunctionalTest
{
    public UploadFileTests(FunctionalTestWebAppFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task UploadFile_ReturnsOkResult()
    {
        // Arrange
        var testFilePath = "TestFiles/testfile.txt";

        // Record current time before the request
        var beforeRequest = DateTime.UtcNow;
        beforeRequest = beforeRequest.AddMilliseconds(-beforeRequest.Millisecond); // Subtract milliseconds

        // Ensure test file exists and get its size
        var fileInfo = new FileInfo(testFilePath);
        Assert.True(fileInfo.Exists, $"Test file not found at {testFilePath}");
        var fileSize = fileInfo.Length;

        using var content = new MultipartFormDataContent();
        using var fileStream = File.OpenRead(testFilePath);
        content.Add(new StreamContent(fileStream), "file", "testfile.txt");

        // Act
        var response = await httpclient.PostAsync($"/files/upload", content);

        var responseString = await response.Content.ReadAsStringAsync();
        
        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299
        
        var jsonResponse = JObject.Parse(responseString);
        
        // Validate JSON structure and property types
        Assert.True(jsonResponse["id"]?.Type == JTokenType.Integer);
        Assert.True(jsonResponse["name"]?.Type == JTokenType.String);
        Assert.True(jsonResponse["status"]?.Type == JTokenType.String);
        Assert.True(jsonResponse["size"]?.Type == JTokenType.Integer);
        Assert.True(jsonResponse["blobUrl"]?.Type == JTokenType.String);
        Assert.True(jsonResponse["created"]?.Type == JTokenType.Date);
        Assert.True(jsonResponse["lastModified"]?.Type == JTokenType.Date || jsonResponse["lastModified"]?.Type == JTokenType.Null);

        // Validate values
        Assert.Equal("testfile.txt", jsonResponse["name"]?.Value<string>());
        Assert.True(jsonResponse["id"]?.Value<long>() > 0);
        Assert.Equal(fileSize, jsonResponse["size"]?.Value<long>());
        Assert.NotNull(jsonResponse["blobUrl"]?.Value<string>());
        Assert.Equal("Uploaded", jsonResponse["status"]?.Value<string>());
        
        /*
        // Parse and validate created date
        var createdStr = jsonResponse["created"]?.Value<string>();
        _debugger.Write($"Created date string: {createdStr}");
        Assert.NotNull(createdStr);
        Assert.True(DateTime.TryParse(createdStr, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out DateTime createdDate));
        
        int result = DateTime.Compare(createdDate, beforeRequest);
        _debugger.Write(result.ToString());
        _debugger.Write($"milliseconds: {createdDate.Millisecond} {beforeRequest.Millisecond}");

        // Ensure created date is not before the request started and not in the past
        Assert.True(createdDate >= beforeRequest, $"Created date {createdDate} should not be before request time {beforeRequest}");
        //Assert.True(createdDate.Kind == DateTimeKind.Utc, "Created date should be in UTC");
        */
    }
}