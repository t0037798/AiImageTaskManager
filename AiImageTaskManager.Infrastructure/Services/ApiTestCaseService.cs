using System.Diagnostics;
using System.Text;
using System.Text.Json;
using AiImageTaskManager.Application.DTOs;
using AiImageTaskManager.Application.Interfaces;
using AiImageTaskManager.Domain.Entities;
using AiImageTaskManager.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AiImageTaskManager.Infrastructure.Services;

public class ApiTestCaseService : IApiTestCaseService
{
    private readonly AppDbContext _dbContext;
    private readonly HttpClient _httpClient;
    private readonly ICurrentUserService _currentUserService;

    public ApiTestCaseService(
        AppDbContext dbContext,
        HttpClient httpClient,
        ICurrentUserService currentUserService)
    {
        _dbContext = dbContext;
        _httpClient = httpClient;
        _currentUserService = currentUserService;
    }

    public async Task<List<ApiTestCaseResponse>> GetAllAsync()
    {
        var userId = _currentUserService.UserId;

        var testCases = await _dbContext.ApiTestCases
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();

        return testCases.Select(MapToTestCaseResponse).ToList();
    }

    public async Task<ApiTestCaseResponse?> GetByIdAsync(int id)
    {
        var userId = _currentUserService.UserId;

        var testCase = await _dbContext.ApiTestCases
            .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);

        return testCase == null ? null : MapToTestCaseResponse(testCase);
    }

    public async Task<ApiTestCaseResponse> CreateAsync(CreateApiTestCaseRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            throw new ArgumentException("Name is required.");
        }

        if (string.IsNullOrWhiteSpace(request.Url))
        {
            throw new ArgumentException("Url is required.");
        }

        var userId = _currentUserService.UserId;

        if (userId <= 0)
        {
            throw new UnauthorizedAccessException("User is not authenticated.");
        }

        var testCase = new ApiTestCase
        {
            UserId = userId,
            Name = request.Name,
            Method = request.Method.ToUpperInvariant(),
            Url = request.Url,
            HeadersJson = request.HeadersJson,
            BodyJson = request.BodyJson,
            ExpectedStatusCode = request.ExpectedStatusCode,
            CreatedAt = DateTime.UtcNow
        };

        _dbContext.ApiTestCases.Add(testCase);
        await _dbContext.SaveChangesAsync();

        return MapToTestCaseResponse(testCase);
    }

    public async Task<ApiTestRunResponse?> RunAsync(int id)
    {
        var userId = _currentUserService.UserId;

        var testCase = await _dbContext.ApiTestCases
            .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);

        if (testCase == null)
        {
            return null;
        }

        var stopwatch = Stopwatch.StartNew();

        var run = new ApiTestRun
        {
            ApiTestCaseId = testCase.Id,
            ExecutedAt = DateTime.UtcNow
        };

        try
        {
            using var requestMessage = new HttpRequestMessage(
                new HttpMethod(testCase.Method),
                testCase.Url);

            if (!string.IsNullOrWhiteSpace(testCase.BodyJson))
            {
                requestMessage.Content = new StringContent(
                    testCase.BodyJson,
                    Encoding.UTF8,
                    "application/json");
            }

            if (!string.IsNullOrWhiteSpace(testCase.HeadersJson))
            {
                var headers = JsonSerializer.Deserialize<Dictionary<string, string>>(
                    testCase.HeadersJson);

                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        requestMessage.Headers.TryAddWithoutValidation(
                            header.Key,
                            header.Value);
                    }
                }
            }

            var response = await _httpClient.SendAsync(requestMessage);
            var responseBody = await response.Content.ReadAsStringAsync();

            stopwatch.Stop();

            run.ActualStatusCode = (int)response.StatusCode;
            run.ActualResponseBody = responseBody;
            run.DurationMs = stopwatch.ElapsedMilliseconds;
            run.IsPassed = run.ActualStatusCode == testCase.ExpectedStatusCode;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();

            run.ActualStatusCode = null;
            run.ActualResponseBody = null;
            run.DurationMs = stopwatch.ElapsedMilliseconds;
            run.IsPassed = false;
            run.ErrorMessage = ex.Message;
        }

        _dbContext.ApiTestRuns.Add(run);
        await _dbContext.SaveChangesAsync();

        return MapToRunResponse(run);
    }

    public async Task<List<ApiTestRunResponse>> GetRunsAsync(int testCaseId)
    {
        var userId = _currentUserService.UserId;

        var testCaseExists = await _dbContext.ApiTestCases
            .AnyAsync(x => x.Id == testCaseId && x.UserId == userId);

        if (!testCaseExists)
        {
            return new List<ApiTestRunResponse>();
        }

        var runs = await _dbContext.ApiTestRuns
            .Where(x => x.ApiTestCaseId == testCaseId)
            .OrderByDescending(x => x.ExecutedAt)
            .ToListAsync();

        return runs.Select(MapToRunResponse).ToList();
    }

    private static ApiTestCaseResponse MapToTestCaseResponse(ApiTestCase testCase)
    {
        return new ApiTestCaseResponse
        {
            Id = testCase.Id,
            Name = testCase.Name,
            Method = testCase.Method,
            Url = testCase.Url,
            HeadersJson = testCase.HeadersJson,
            BodyJson = testCase.BodyJson,
            ExpectedStatusCode = testCase.ExpectedStatusCode,
            CreatedAt = testCase.CreatedAt
        };
    }

    private static ApiTestRunResponse MapToRunResponse(ApiTestRun run)
    {
        return new ApiTestRunResponse
        {
            Id = run.Id,
            ApiTestCaseId = run.ApiTestCaseId,
            ActualStatusCode = run.ActualStatusCode,
            ActualResponseBody = run.ActualResponseBody,
            IsPassed = run.IsPassed,
            ErrorMessage = run.ErrorMessage,
            DurationMs = run.DurationMs,
            ExecutedAt = run.ExecutedAt
        };
    }
}