namespace AiImageTaskManager.Domain.Entities;

public class User
{
    public int Id { get; set; }

    public string Email { get; set; } = string.Empty;

    public string DisplayName { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public List<ImageGenerationTask> ImageGenerationTasks { get; set; } = new();

    public List<ApiTestCase> ApiTestCases { get; set; } = new();
}