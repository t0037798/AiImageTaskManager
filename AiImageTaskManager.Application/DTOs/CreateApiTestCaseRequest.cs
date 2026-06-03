namespace AiImageTaskManager.Application.DTOs;

public class CreateApiTestCaseRequest
{
    public string Name { get; set; } = string.Empty;

    public string Method { get; set; } = "GET";

    public string Url { get; set; } = string.Empty;

    public string? HeadersJson { get; set; }

    public string? BodyJson { get; set; }

    public int ExpectedStatusCode { get; set; } = 200;
}