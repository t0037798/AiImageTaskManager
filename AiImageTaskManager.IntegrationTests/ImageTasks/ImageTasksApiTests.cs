using System.Net;
using System.Net.Http.Json;
using AiImageTaskManager.IntegrationTests.Factories;

namespace AiImageTaskManager.IntegrationTests.ImageTasks;

public class ImageTasksApiTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public ImageTasksApiTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task CreateImageTask_ShouldReturnCreated()
    {
        var request = new
        {
            prompt = "a realistic train running on railway tracks",
            negativePrompt = "blurry, low quality",
            width = 512,
            height = 512,
            steps = 20,
            cfgScale = 7.0,
            seed = 12345
        };

        var response = await _client.PostAsJsonAsync("/api/image-tasks", request);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task GetImageTasks_ShouldReturnOk()
    {
        var response = await _client.GetAsync("/api/image-tasks");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetNonExistingImageTask_ShouldReturnNotFound()
    {
        var response = await _client.GetAsync("/api/image-tasks/999999");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetImageTaskById_ShouldReturnOk()
    {
        var request = new
        {
            prompt = "a realistic train running on railway tracks",
            negativePrompt = "blurry, low quality",
            width = 512,
            height = 512,
            steps = 20,
            cfgScale = 7.0,
            seed = 12345
        };

        var createResponse = await _client.PostAsJsonAsync("/api/image-tasks", request);

        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);

        var createdTask = await createResponse.Content.ReadFromJsonAsync<ImageTaskResponseForTest>();

        Assert.NotNull(createdTask);

        var getResponse = await _client.GetAsync($"/api/image-tasks/{createdTask!.Id}");

        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
    }

    private class ImageTaskResponseForTest
    {
        public int Id { get; set; }
    }
}