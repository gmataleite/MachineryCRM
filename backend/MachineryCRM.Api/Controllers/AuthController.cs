using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Asp.Versioning;
using MachineryCRM.Application.Interfaces;

namespace MachineryCRM.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IAppUserService _appUserService;

    public AuthController(IConfiguration configuration, IAppUserService appUserService)
    {
        _configuration = configuration;
        _appUserService = appUserService;
    }

[HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        // 1. Tenta buscar o usuário no banco de dados real
        var user = await _appUserService.GetByEmailAsync(request.Email);
        
        if (user != null && user.IsActive)
        {
            // Em produção, usar BCrypt.Verify(request.Password, user.PasswordHash)
            var token = GenerateJwtToken(user.Email, user.Role);
            return Ok(new { token });
        }

        // 2. Fallback de Desenvolvimento (Mock Bypass)
        // Isso garante que você e o front não fiquem travados enquanto o fluxo de cadastro não existe
        if (request.Email == "admin@email.com.br" && request.Password == "admin123")
        {
            var token = GenerateJwtToken(request.Email, "Admin");
            return Ok(new { token });
        }
        
        if (request.Email == "tecnico@email.com.br" && request.Password == "tecnico123")
        {
            var token = GenerateJwtToken(request.Email, "Technician");
            return Ok(new { token });
        }

        return Unauthorized(new { message = "Invalid credentials or inactive user." });
    }

    private string GenerateJwtToken(string email, string role)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var secretKey = jwtSettings["Secret"] ?? throw new InvalidOperationException("JWT Secret is missing.");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Role, role)
        };

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

public class LoginRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}