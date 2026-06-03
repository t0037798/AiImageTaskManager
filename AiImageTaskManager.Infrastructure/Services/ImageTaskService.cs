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
    private readonly ICurrentUserService _currentUserService;

    public ImageTaskService(
        AppDbContext dbContext,
        ICurrentUserService currentUserService)
    {
        _dbContext = dbContext;
        _currentUserService = currentUserService;
    }

    public async Task<List<ImageTaskResponse>> GetAllAsync()
    {
        var userId = _currentUserService.UserId;

        var tasks = await _dbContext.ImageGenerationTasks
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();

        return tasks.Select(MapToResponse).ToList();
    }

    public async Task<ImageTaskResponse?> GetByIdAsync(int id)
    {
        var userId = _currentUserService.UserId;

        var task = await _dbContext.ImageGenerationTasks
            .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);

        return task == null ? null : MapToResponse(task);
    }

    public async Task<ImageTaskResponse> CreateAsync(CreateImageTaskRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Prompt))
        {
            throw new ArgumentException("Prompt is required.");
        }

        var userId = _currentUserService.UserId;

        if (userId <= 0)
        {
            throw new UnauthorizedAccessException("User is not authenticated.");
        }

        var task = new ImageGenerationTask
        {
            UserId = userId,
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
        var userId = _currentUserService.UserId;

        var task = await _dbContext.ImageGenerationTasks
            .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);

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

    public async Task<List<GeneratedImageResponse>> GetImagesByTaskIdAsync(int taskId)
    {
        var userId = _currentUserService.UserId;

        var images = await _dbContext.GeneratedImages
            .Where(x => x.ImageGenerationTaskId == taskId &&
                        x.ImageGenerationTask != null &&
                        x.ImageGenerationTask.UserId == userId)
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
}