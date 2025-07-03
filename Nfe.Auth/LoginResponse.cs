namespace Nfe.Auth;

public class LoginResponse
{
    public string Token { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
}
