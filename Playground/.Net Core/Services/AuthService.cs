using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using NetCoreApp.Common;
using NetCoreApp.DTOs;

namespace NetCoreApp.Services;

public class AuthService : IAuthService
{
    private readonly IConfiguration _configuration;

    public AuthService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public ServiceResult<LoginResponseDto> Authenticate(LoginRequestDto request)
    {
        var expectedUsername = _configuration["MockAuth:Username"] ?? "admin";
        var expectedPassword = _configuration["MockAuth:Password"] ?? "admin123";

        if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
        {
            return ServiceResult<LoginResponseDto>.Fail("Username and password are required.", 400);
        }

        if (request.Username != expectedUsername || request.Password != expectedPassword)
        {
            return ServiceResult<LoginResponseDto>.Fail("Invalid username or password.", 401);
        }

        var secretKey = _configuration["Jwt:Key"] ?? "DefaultFallbackSecretKeyWithSufficientLength1234567890!";
        var issuer = _configuration["Jwt:Issuer"] ?? "NetCoreApp";
        var audience = _configuration["Jwt:Audience"] ?? "NetCoreAppUsers";
        var expiryHours = int.TryParse(_configuration["Jwt:ExpiryInHours"], out var hours) ? hours : 24;

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(secretKey);
        var expires = DateTime.UtcNow.AddHours(expiryHours);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, request.Username),
                new Claim(ClaimTypes.Role, "Admin"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            }),
            Expires = expires,
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        return ServiceResult<LoginResponseDto>.Ok(new LoginResponseDto
        {
            Token = tokenString,
            Username = request.Username,
            Expiration = expires
        }, "Authentication successful.");
    }
}
