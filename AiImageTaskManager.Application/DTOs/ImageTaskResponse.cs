namespace AiImageTaskManager.Application.DTOs;

public class ImageTaskResponse
{
    public int Id { get; set; }

    public string Prompt { get; set; } = string.Empty;

    public string? NegativePrompt { get; set; }

    public int Width { get; set; }

    public int Height { get; set; }

    public int Steps { get; set; }

    public double CfgScale { get; set; }

    public int? Seed { get; set; }

    public string Status { get; set; } = string.Empty;

    public string? ErrorMessage { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? StartedAt { get; set; }

    public DateTime? CompletedAt { get; set; }
}