namespace AiImageTaskManager.Application.DTOs;

public class ApiTestCaseResponse
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Method { get; set; } = string.Empty;

    public string Url { get; set; } = string.Empty;

    public string? HeadersJson { get; set; }

    public string? BodyJson { get; set; }

    public int ExpectedStatusCode { get; set; }

    public DateTime CreatedAt { get; set; }
}