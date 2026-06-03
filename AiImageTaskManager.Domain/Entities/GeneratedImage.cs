namespace AiImageTaskManager.Domain.Entities;

public class GeneratedImage
{
    /* public int Id { get; set; }

     public int ImageGenerationTaskId { get; set; }

     public string ImagePath { get; set; } = string.Empty;

     public string? ThumbnailPath { get; set; }

     public long? FileSize { get; set; }

     public int Width { get; set; }

     public int Height { get; set; }

     public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

     public ImageGenerationTask? ImageGenerationTask { get; set; }*/

    /*
        Id                    圖片紀錄 ID
        ImageGenerationTaskId 對應哪一個生成任務
        ImagePath             圖片路徑
        ThumbnailPath         縮圖路徑
        FileSize              檔案大小
        Width / Height        圖片尺寸
        CreatedAt             建立時間
     */
    public int Id { get; set; }

    public int ImageGenerationTaskId { get; set; }

    public string ImagePath { get; set; } = string.Empty;

    public string? ThumbnailPath { get; set; }

    public long? FileSize { get; set; }

    public int Width { get; set; }

    public int Height { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ImageGenerationTask? ImageGenerationTask { get; set; }

}