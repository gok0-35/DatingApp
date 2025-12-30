namespace DatingApp.API.DTOs.Auth;

public record RegisterDto
{
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public DateTime BirthDate { get; init; }
    public string Gender { get; init; } = string.Empty;
}