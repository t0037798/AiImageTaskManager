using AiImageTaskManager.Application.DTOs;
using AiImageTaskManager.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AiImageTaskManager.Api.Controllers;

[ApiController]
[Route("api/test-cases")]
public class ApiTestCasesController : ControllerBase
{
    private readonly IApiTestCaseService _apiTestCaseService;

    public ApiTestCasesController(IApiTestCaseService apiTestCaseService)
    {
        _apiTestCaseService = apiTestCaseService;
    }

    [HttpGet]
    public async Task<ActionResult<List<ApiTestCaseResponse>>> GetAll()
    {
        var testCases = await _apiTestCaseService.GetAllAsync();

        return Ok(testCases);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiTestCaseResponse>> GetById(int id)
    {
        var testCase = await _apiTestCaseService.GetByIdAsync(id);

        if (testCase == null)
        {
            return NotFound();
        }

        return Ok(testCase);
    }

    [HttpPost]
    public async Task<ActionResult<ApiTestCaseResponse>> Create(CreateApiTestCaseRequest request)
    {
        try
        {
            var testCase = await _apiTestCaseService.CreateAsync(request);

            return CreatedAtAction(nameof(GetById), new { id = testCase.Id }, testCase);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("{id:int}/run")]
    public async Task<ActionResult<ApiTestRunResponse>> Run(int id)
    {
        var result = await _apiTestCaseService.RunAsync(id);

        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpGet("{id:int}/runs")]
    public async Task<ActionResult<List<ApiTestRunResponse>>> GetRuns(int id)
    {
        var testCase = await _apiTestCaseService.GetByIdAsync(id);

        if (testCase == null)
        {
            return NotFound();
        }

        var runs = await _apiTestCaseService.GetRunsAsync(id);

        return Ok(runs);
    }
}