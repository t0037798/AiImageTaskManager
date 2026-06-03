using AiImageTaskManager.Application.DTOs;
using AiImageTaskManager.Application.Interfaces;
using AiImageTaskManager.Domain.Entities;
using AiImageTaskManager.Domain.Enums;
using AiImageTaskManager.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AiImageTaskManager.Infrastructure.Services;


public class ImageTaskService : IImageTaskService
{
    private readonly AppDbContext _dbContext;

    public ImageTaskService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<ImageTaskResponse>> GetAllAsync()
    {
        var tasks = await _dbContext.ImageGenerationTasks
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();

        return tasks.Select(MapToResponse).ToList();
    }

    public async Task<ImageTaskResponse?> GetByIdAsync(int id)
    {
        var task = await _dbContext.ImageGenerationTasks
            .FirstOrDefaultAsync(x => x.Id == id);

        return task == null ? null : MapToResponse(task);
    }

    public async Task<ImageTaskResponse> CreateAsync(CreateImageTaskRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Prompt))
        {
            throw new ArgumentException("Prompt is required.");
        }

        var task = new ImageGenerationTask
        {
            Prompt = request.Prompt,
            NegativePrompt = request.NegativePrompt,
            Width = request.Width,
            Height = request.Height,
            Steps = request.Steps,
            CfgScale = request.CfgScale,
            Seed = request.Seed,
            Status = ImageTaskStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        _dbContext.ImageGenerationTasks.Add(task);
        await _dbContext.SaveChangesAsync();

        return MapToResponse(task);
    }

    public async Task<bool> CancelAsync(int id)
    {
        var task = await _dbContext.ImageGenerationTasks
            .FirstOrDefaultAsync(x => x.Id == id);

        if (task == null)
        {
            return false;
        }

        if (task.Status is ImageTaskStatus.Completed or ImageTaskStatus.Failed)
        {
            throw new InvalidOperationException("Completed or failed task cannot be cancelled.");
        }

        task.Status = ImageTaskStatus.Cancelled;
        await _dbContext.SaveChangesAsync();

        return true;
    }

    private static ImageTaskResponse MapToResponse(ImageGenerationTask task)
    {
        return new ImageTaskResponse
        {
            Id = task.Id,
            Prompt = task.Prompt,
            NegativePrompt = task.NegativePrompt,
            Width = task.Width,
            Height = task.Height,
            Steps = task.Steps,
            CfgScale = task.CfgScale,
            Seed = task.Seed,
            Status = task.Status.ToString(),
            ErrorMessage = task.ErrorMessage,
            CreatedAt = task.CreatedAt,
            StartedAt = task.StartedAt,
            CompletedAt = task.CompletedAt
        };
    }

    public async Task<List<GeneratedImageResponse>> GetImagesByTaskIdAsync(int taskId)
    {
        var images = await _dbContext.GeneratedImages
            .Where(x => x.ImageGenerationTaskId == taskId)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();

        return images.Select(x => new GeneratedImageResponse
        {
            Id = x.Id,
            ImageGenerationTaskId = x.ImageGenerationTaskId,
            ImagePath = x.ImagePath,
            ThumbnailPath = x.ThumbnailPath,
            FileSize = x.FileSize,
            Width = x.Width,
            Height = x.Height,
            CreatedAt = x.CreatedAt
        }).ToList();
    }
}