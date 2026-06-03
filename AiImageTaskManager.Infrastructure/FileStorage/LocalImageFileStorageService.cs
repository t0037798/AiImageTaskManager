/*using AiImageTaskManager.Application.Interfaces;
using Microsoft.AspNetCore.Hosting;

namespace AiImageTaskManager.Infrastructure.FileStorage;

public class LocalImageFileStorageService : IImageFileStorageService
{
    private readonly IWebHostEnvironment _environment;

    public LocalImageFileStorageService(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    public async Task<string> SaveMockImageAsync(
        int taskId,
        int width,
        int height,
        CancellationToken cancellationToken = default)
    {
        var fileName = $"task-{taskId}-{DateTime.UtcNow:yyyyMMddHHmmss}.png";

        var relativeFolder = Path.Combine("images", "generated");

        var webRootPath = _environment.WebRootPath;

        if (string.IsNullOrWhiteSpace(webRootPath))
        {
            webRootPath = Path.Combine(_environment.ContentRootPath, "wwwroot");
        }

        var folderPath = Path.Combine(webRootPath, relativeFolder);

        Directory.CreateDirectory(folderPath);

        var filePath = Path.Combine(folderPath, fileName);

        var pngBytes = CreateMockPngBytes();

        await File.WriteAllBytesAsync(filePath, pngBytes, cancellationToken);

        return $"/images/generated/{fileName}";
    }

    private static byte[] CreateMockPngBytes()
    {
        // 這是一張 1x1 PNG 圖片，用來先測試檔案儲存流程。
        const string base64Png =
            "iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAQAAAC1HAwCAAAAC0lEQVR42mP8/x8AAwMCAO+/p9sAAAAASUVORK5CYII=";

        return Convert.FromBase64String(base64Png);
    }
}*/

using AiImageTaskManager.Application.DTOs;
using AiImageTaskManager.Application.Interfaces;
using Microsoft.AspNetCore.Hosting;

namespace AiImageTaskManager.Infrastructure.FileStorage;

public class LocalImageFileStorageService : IImageFileStorageService
{
    private readonly IWebHostEnvironment _environment;

    public LocalImageFileStorageService(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    public async Task<ImageFileSaveResult> SaveMockImageAsync(
        int taskId,
        int width,
        int height,
        CancellationToken cancellationToken = default)
    {
        var fileName = $"task-{taskId}-{DateTime.UtcNow:yyyyMMddHHmmss}.png";

        var relativeFolder = Path.Combine("images", "generated");

        var webRootPath = _environment.WebRootPath;

        if (string.IsNullOrWhiteSpace(webRootPath))
        {
            webRootPath = Path.Combine(_environment.ContentRootPath, "wwwroot");
        }

        var folderPath = Path.Combine(webRootPath, relativeFolder);

        Directory.CreateDirectory(folderPath);

        var filePath = Path.Combine(folderPath, fileName);

        var pngBytes = CreateMockPngBytes();

        await File.WriteAllBytesAsync(filePath, pngBytes, cancellationToken);

        var fileInfo = new FileInfo(filePath);

        return new ImageFileSaveResult
        {
            ImagePath = $"/images/generated/{fileName}",
            FileSize = fileInfo.Length
        };
    }

    private static byte[] CreateMockPngBytes()
    {
        const string base64Png =
            "iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAQAAAC1HAwCAAAAC0lEQVR42mP8/x8AAwMCAO+/p9sAAAAASUVORK5CYII=";

        return Convert.FromBase64String(base64Png);
    }
}