using AiImageTaskManager.Domain.Enums;

namespace AiImageTaskManager.Domain.Entities;

public class ImageGenerationTask
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public User? User { get; set; }

    public string Prompt { get; set; } = string.Empty;

    public string? NegativePrompt { get; set; }

    public int Width { get; set; } = 512;

    public int Height { get; set; } = 512;

    public int Steps { get; set; } = 20;

    public double CfgScale { get; set; } = 7.0;

    public int? Seed { get; set; }

    public ImageTaskStatus Status { get; set; } = ImageTaskStatus.Pending;

    public string? ErrorMessage { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? StartedAt { get; set; }

    public DateTime? CompletedAt { get; set; }

    public List<GeneratedImage> GeneratedImages { get; set; } = new();
}