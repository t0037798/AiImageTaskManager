namespace AiImageTaskManager.Application.DTOs;

public class CurrentUserResponse
{
    public int UserId { get; set; }

    public string Email { get; set; } = string.Empty;

    public string DisplayName { get; set; } = string.Empty;
}