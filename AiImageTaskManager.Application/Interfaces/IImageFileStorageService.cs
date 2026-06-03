using AiImageTaskManager.Application.DTOs;

namespace AiImageTaskManager.Application.Interfaces;

public interface IImageFileStorageService
{
    /*Task<string> SaveMockImageAsync(
        int taskId,
        int width,
        int height,
        CancellationToken cancellationToken = default);
    */
    Task<ImageFileSaveResult> SaveMockImageAsync(
      int taskId,
      int width,
      int height,
      CancellationToken cancellationToken = default);
}