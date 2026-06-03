using AiImageTaskManager.Application.DTOs;
using AiImageTaskManager.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AiImageTaskManager.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/image-tasks")]
public class ImageTasksController : ControllerBase
{
    private readonly IImageTaskService _imageTaskService;

    public ImageTasksController(IImageTaskService imageTaskService)
    {
        _imageTaskService = imageTaskService;
    }

    [HttpGet]
    public async Task<ActionResult<List<ImageTaskResponse>>> GetAll()
    {
        var tasks = await _imageTaskService.GetAllAsync();

        return Ok(tasks);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ImageTaskResponse>> GetById(int id)
    {
        var task = await _imageTaskService.GetByIdAsync(id);

        if (task == null)
        {
            return NotFound();
        }

        return Ok(task);
    }

    [HttpPost]
    public async Task<ActionResult<ImageTaskResponse>> Create(CreateImageTaskRequest request)
    {
        try
        {
            var task = await _imageTaskService.CreateAsync(request);

            return CreatedAtAction(nameof(GetById), new { id = task.Id }, task);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("{id:int}/cancel")]
    public async Task<IActionResult> Cancel(int id)
    {
        try
        {
            var result = await _imageTaskService.CancelAsync(id);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{id:int}/images")]
    public async Task<ActionResult<List<GeneratedImageResponse>>> GetImages(int id)
    {
        var task = await _imageTaskService.GetByIdAsync(id);

        if (task == null)
        {
            return NotFound();
        }

        var images = await _imageTaskService.GetImagesByTaskIdAsync(id);

        return Ok(images);
    }
}