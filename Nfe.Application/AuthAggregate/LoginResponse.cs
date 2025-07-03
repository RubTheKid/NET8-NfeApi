namespace Nfe.Application.AuthAggregate;

public record LoginResponse
{
    public string Token { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public DateTime ExpiresAt { get; init; }
    public bool Success { get; init; }
    public string Message { get; init; } = string.Empty;
}
