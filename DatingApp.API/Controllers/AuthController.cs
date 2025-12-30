using DatingApp.API.DTOs.Auth;
using DatingApp.API.Models;
using DatingApp.API.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DatingApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly JwtSettings _jwtSettings;

    public AuthController(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        IOptions<JwtSettings> jwtSettings)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtSettings = jwtSettings.Value;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        // Email kontrolü
        if (await _userManager.FindByEmailAsync(dto.Email) != null)
        {
            return BadRequest(new { message = "Bu email zaten kullanılıyor." });
        }

        // Yaş kontrolü (18 yaşından küçük olmasın)
        var age = DateTime.UtcNow.Year - dto.BirthDate.Year;
        if (age < 18)
        {
            return BadRequest(new { message = "18 yaşından küçükler kayıt olamaz." });
        }

        // User oluştur
        var user = new User
        {
            UserName = dto.Email,
            Email = dto.Email,
            Name = dto.Name,
            BirthDate = dto.BirthDate,
            Gender = dto.Gender,
            CreatedAt = DateTime.UtcNow
        };

        var result = await _userManager.CreateAsync(user, dto.Password);

        if (!result.Succeeded)
        {
            return BadRequest(new { errors = result.Errors });
        }

        // JWT token oluştur
        var token = GenerateJwtToken(user);

        return Ok(new AuthResponseDto
        {
            Token = token,
            UserId = user.Id,
            Email = user.Email,
            Name = user.Name
        });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);

        if (user == null)
        {
            return Unauthorized(new { message = "Email veya şifre hatalı." });
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);

        if (!result.Succeeded)
        {
            return Unauthorized(new { message = "Email veya şifre hatalı." });
        }

        // Email null check
        if (string.IsNullOrEmpty(user.Email))
        {
            return StatusCode(500, new { message = "Kullanıcı email'i bulunamadı." });
        }

        // JWT token oluştur
        var token = GenerateJwtToken(user);

        return Ok(new AuthResponseDto
        {
            Token = token,
            UserId = user.Id,
            Email = user.Email,
            Name = user.Name
        });
    }

    private string GenerateJwtToken(User user)
    {
        // Email null olamaz, ama yine de kontrol edelim
        if (string.IsNullOrEmpty(user.Email))
        {
            throw new InvalidOperationException("User email cannot be null.");
        }

        var claims = new[]
        {
        new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
        new Claim(JwtRegisteredClaimNames.Email, user.Email),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim("name", user.Name)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationInMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}