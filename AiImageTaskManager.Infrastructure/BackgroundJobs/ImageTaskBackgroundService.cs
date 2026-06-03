using AiImageTaskManager.Application.Interfaces;

using AiImageTaskManager.Domain.Entities;

using AiImageTaskManager.Domain.Enums;
using AiImageTaskManager.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AiImageTaskManager.Infrastructure.BackgroundJobs;

public class ImageTaskBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<ImageTaskBackgroundService> _logger;

    public ImageTaskBackgroundService(
        IServiceProvider serviceProvider,
        ILogger<ImageTaskBackgroundService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Image task background service started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ProcessPendingTaskAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing image task.");
            }

            await Task.Delay(TimeSpan.FromSeconds(3), stoppingToken);
        }
    }

    private async Task ProcessPendingTaskAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var task = await dbContext.ImageGenerationTasks
            .Where(x => x.Status == ImageTaskStatus.Pending)
            .OrderBy(x => x.CreatedAt)
            .FirstOrDefaultAsync(cancellationToken);

        if (task == null)
        {
            return;
        }

        _logger.LogInformation("Start processing image task {TaskId}.", task.Id);

        task.Status = ImageTaskStatus.Running;
        task.StartedAt = DateTime.UtcNow;
        await dbContext.SaveChangesAsync(cancellationToken);

        await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);

        /*task.Status = ImageTaskStatus.Completed;
        task.CompletedAt = DateTime.UtcNow;
        await dbContext.SaveChangesAsync(cancellationToken);*/
        /*var generatedImage = new GeneratedImage
        {
            ImageGenerationTaskId = task.Id,
            ImagePath = "/images/mock-result.png",
            ThumbnailPath = "/images/mock-result-thumb.png",
            FileSize = 0,
            Width = task.Width,
            Height = task.Height,
            CreatedAt = DateTime.UtcNow
        };*/
        /*var imageFileStorageService = scope.ServiceProvider.GetRequiredService<IImageFileStorageService>();

        var imagePath = await imageFileStorageService.SaveMockImageAsync(
            task.Id,
            task.Width,
            task.Height,
            cancellationToken);

        var generatedImage = new GeneratedImage
        {
            ImageGenerationTaskId = task.Id,
            ImagePath = imagePath,
            ThumbnailPath = imagePath,
            FileSize = 0,
            Width = task.Width,
            Height = task.Height,
            CreatedAt = DateTime.UtcNow
        };*/
        var imageFileStorageService = scope.ServiceProvider.GetRequiredService<IImageFileStorageService>();

        var imageSaveResult = await imageFileStorageService.SaveMockImageAsync(
            task.Id,
            task.Width,
            task.Height,
            cancellationToken);

        var generatedImage = new GeneratedImage
        {
            ImageGenerationTaskId = task.Id,
            ImagePath = imageSaveResult.ImagePath,
            ThumbnailPath = imageSaveResult.ImagePath,
            FileSize = imageSaveResult.FileSize,
            Width = task.Width,
            Height = task.Height,
            CreatedAt = DateTime.UtcNow
        };


        dbContext.GeneratedImages.Add(generatedImage);

        task.Status = ImageTaskStatus.Completed;
        task.CompletedAt = DateTime.UtcNow;

        await dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Image task {TaskId} completed.", task.Id);
    }
}