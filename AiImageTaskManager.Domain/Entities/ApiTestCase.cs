namespace AiImageTaskManager.Domain.Entities;

public class ApiTestCase
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public User? User { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Method { get; set; } = "GET";

    public string Url { get; set; } = string.Empty;

    public string? HeadersJson { get; set; }

    public string? BodyJson { get; set; }

    public int ExpectedStatusCode { get; set; } = 200;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public List<ApiTestRun> Runs { get; set; } = new();
}