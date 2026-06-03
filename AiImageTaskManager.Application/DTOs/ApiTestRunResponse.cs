namespace AiImageTaskManager.Application.DTOs;

public class ApiTestRunResponse
{
    public int Id { get; set; }

    public int ApiTestCaseId { get; set; }

    public int? ActualStatusCode { get; set; }

    public string? ActualResponseBody { get; set; }

    public bool IsPassed { get; set; }

    public string? ErrorMessage { get; set; }

    public long DurationMs { get; set; }

    public DateTime ExecutedAt { get; set; }
}