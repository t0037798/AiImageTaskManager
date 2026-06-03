namespace AiImageTaskManager.Application.DTOs;

public class GeneratedImageResponse
{
    public int Id { get; set; }

    public int ImageGenerationTaskId { get; set; }

    public string ImagePath { get; set; } = string.Empty;

    public string? ThumbnailPath { get; set; }

    public long? FileSize { get; set; }

    public int Width { get; set; }

    public int Height { get; set; }

    public DateTime CreatedAt { get; set; }
}