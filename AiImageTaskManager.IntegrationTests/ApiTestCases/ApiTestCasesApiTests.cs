using System.Net;
using System.Net.Http.Json;
using AiImageTaskManager.IntegrationTests.Factories;
using AiImageTaskManager.IntegrationTests.Helpers;
using AiImageTaskManager.Application.DTOs;

namespace AiImageTaskManager.IntegrationTests.ApiTestCases;

public class ApiTestCasesApiTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public ApiTestCasesApiTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
        AuthTestHelper.AuthenticateAsync(_client).GetAwaiter().GetResult();
    }

    [Fact]
    public async Task CreateApiTestCase_ShouldReturnCreated()
    {
        var request = new
        {
            name = "Get all image tasks",
            method = "GET",
            url = "https://localhost/api/image-tasks",
            headersJson = (string?)null,
            bodyJson = (string?)null,
            expectedStatusCode = 200
        };

        var response = await _client.PostAsJsonAsync("/api/test-cases", request);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task GetApiTestCases_ShouldReturnOk()
    {
        var response = await _client.GetAsync("/api/test-cases");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetNonExistingApiTestCase_ShouldReturnNotFound()
    {
        var response = await _client.GetAsync("/api/test-cases/999999");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetApiTestCaseById_ShouldReturnOk()
    {
        var request = new
        {
            name = "Get all image tasks",
            method = "GET",
            url = "https://localhost/api/image-tasks",
            headersJson = (string?)null,
            bodyJson = (string?)null,
            expectedStatusCode = 200
        };

        var createResponse = await _client.PostAsJsonAsync("/api/test-cases", request);

        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);

        var createdTestCase = await createResponse.Content.ReadFromJsonAsync<ApiTestCaseResponse>();

        Assert.NotNull(createdTestCase);
        Assert.True(createdTestCase!.Id > 0);

        var getResponse = await _client.GetAsync($"/api/test-cases/{createdTestCase.Id}");

        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
    }

    /*private class ApiTestCaseResponseForTest
    {
        public int Id { get; set; }
    }*/
}