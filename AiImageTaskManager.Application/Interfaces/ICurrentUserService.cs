namespace AiImageTaskManager.Application.Interfaces;

public interface ICurrentUserService
{
    int UserId { get; }

    string Email { get; }

    string DisplayName { get; }
}