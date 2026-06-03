namespace AiImageTaskManager.Domain.Enums;

public enum ImageTaskStatus
{
    Pending = 0,
    Running = 1,
    Completed = 2,
    Failed = 3,
    Cancelled = 4
}