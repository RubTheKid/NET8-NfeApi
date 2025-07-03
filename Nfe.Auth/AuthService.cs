using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Nfe.Auth.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Nfe.Auth;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHashService _passwordHashService;
    private readonly IConfiguration _configuration;

    public AuthService(
        IUserRepository userRepository,
        IPasswordHashService passwordHashService,
        IConfiguration configuration)
    {
        _userRepository = userRepository;
        _passwordHashService = passwordHashService;
        _configuration = configuration;
    }

    public async Task<LoginResponse> LoginAsync(LoginModel request)
    {
        try
        {
            var user = await _userRepository.GetByEmail(request.Email);

            if (user == null || !user.IsActive)
            {
                return new LoginResponse
                {
                    Success = false,
                    Message = "Email ou senha inválidos"
                };
            }

            if (!_passwordHashService.VerifyPassword(request.Password, user.PasswordHash))
            {
                return new LoginResponse
                {
                    Success = false,
                    Message = "Email ou senha inválidos"
                };
            }

            user.UpdateLastLogin();
            await _userRepository.Update(user);
            await _userRepository.SaveChangesAsync();

            var token = GenerateJwtToken(user.Email, user.Id);
            var expiresAt = DateTime.UtcNow.AddHours(24);

            return new LoginResponse
            {
                Success = true,
                Token = token,
                Email = user.Email,
                ExpiresAt = expiresAt,
                Message = "Login realizado com sucesso"
            };
        }
        catch (Exception ex)
        {
            return new LoginResponse
            {
                Success = false,
                Message = $"Erro interno do servidor: {ex.Message}"
            };
        }
    }

    public async Task<RegisterResponse> RegisterAsync(RegisterModel request)
    {
        try
        {
            if (request.Password != request.ConfirmPassword)
            {
                return new RegisterResponse
                {
                    Success = false,
                    Message = "As senhas não coincidem"
                };
            }

            if (await _userRepository.EmailExists(request.Email))
            {
                return new RegisterResponse
                {
                    Success = false,
                    Message = "Este email já está cadastrado"
                };
            }

            var hashedPassword = _passwordHashService.HashPassword(request.Password);
            var user = new User(request.Email, hashedPassword);

            await _userRepository.Create(user);
            await _userRepository.SaveChangesAsync();

            return new RegisterResponse
            {
                Success = true,
                UserId = user.Id,
                Email = user.Email,
                Message = "Usuário criado com sucesso"
            };
        }
        catch (Exception ex)
        {
            return new RegisterResponse
            {
                Success = false,
                Message = $"Erro interno do servidor: {ex.Message}"
            };
        }
    }

    public string GenerateJwtToken(string email, Guid userId)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey not configured");
        var issuer = jwtSettings["Issuer"] ?? "NfeApi";
        var audience = jwtSettings["Audience"] ?? "NfeApi";

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(ClaimTypes.Email, email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
        };

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(24),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
