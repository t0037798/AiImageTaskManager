namespace AiImageTaskManager.Application.DTOs;

public class CreateImageTaskRequest
{
    public string Prompt { get; set; } = string.Empty;

    public string? NegativePrompt { get; set; }

    public int Width { get; set; } = 512;

    public int Height { get; set; } = 512;

    public int Steps { get; set; } = 20;

    public double CfgScale { get; set; } = 7.0;

    public int? Seed { get; set; }
}