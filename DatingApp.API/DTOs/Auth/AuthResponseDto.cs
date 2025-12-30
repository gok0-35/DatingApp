namespace DatingApp.API.DTOs.Auth;

public record AuthResponseDto
{
    public string Token { get; init; } = string.Empty;
    public int UserId { get; init; }
    public string Email { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
}