using AiImageTaskManager.Application.DTOs;

namespace AiImageTaskManager.Application.Interfaces;

public interface IApiTestCaseService
{
    Task<List<ApiTestCaseResponse>> GetAllAsync();

    Task<ApiTestCaseResponse?> GetByIdAsync(int id);

    Task<ApiTestCaseResponse> CreateAsync(CreateApiTestCaseRequest request);

    Task<ApiTestRunResponse?> RunAsync(int id);

    Task<List<ApiTestRunResponse>> GetRunsAsync(int testCaseId);
}