using AiImageTaskManager.Application.DTOs;

namespace AiImageTaskManager.Application.Interfaces;

public interface IImageTaskService
{
    Task<List<ImageTaskResponse>> GetAllAsync();

    Task<ImageTaskResponse?> GetByIdAsync(int id);

    Task<ImageTaskResponse> CreateAsync(CreateImageTaskRequest request);

    Task<bool> CancelAsync(int id);

    Task<List<GeneratedImageResponse>> GetImagesByTaskIdAsync(int taskId);
}