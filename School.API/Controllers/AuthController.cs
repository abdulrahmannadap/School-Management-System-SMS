using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using School.Application.DTOs.Auth;
using School.Application.Interfaces;
using School.Persistence;

namespace School.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IJwtService jwtService, AppDbContext db) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        var hash = SeedData.Hash(request.Password);

        var user = await db.Users.FirstOrDefaultAsync(u =>
            u.Email == request.Email &&
            u.PasswordHash == hash &&
            u.IsActive);

        if (user is null)
            return Unauthorized(new { message = "Invalid email or password." });

        return Ok(new AuthResponseDto
        {
            Token     = jwtService.GenerateToken(user),
            FullName  = user.FullName,
            Role      = user.Role.ToString(),
            ExpiresAt = DateTime.UtcNow.AddHours(8)
        });
    }
}
