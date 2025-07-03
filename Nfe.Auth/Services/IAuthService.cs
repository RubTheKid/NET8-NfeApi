using Nfe.Auth.Models;

namespace Nfe.Auth.Services;

public interface IAuthService
{
    Task<LoginResponse> LoginAsync(LoginModel request);
    Task<RegisterResponse> RegisterAsync(RegisterModel request);
    string GenerateJwtToken(string email, Guid userId);
}