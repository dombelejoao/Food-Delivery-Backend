using Microsoft.AspNetCore.Authorization;
using FoodDelivery.BusinessLogic.DTOs;
using FoodDelivery.BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FoodDelivery.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // all endpoints require authentication
public class AuthController : ControllerBase
{
    [HttpGet]
    public IActionResult GetOrders()
    {
        // For now, just return a placeholder
        return Ok(new { message = "Orders retrieved successfully." });
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public IActionResult DeleteOrder(Guid id)
    {
        // Placeholder admin-only action
        return Ok($"Order {id} deleted by admin.");
    }

    // Open endpoint (no auth)
    [HttpGet("test")]
    public IActionResult Test()
    {
        return Ok("This is a public endpoint.");
    }

    // Protected endpoint (requires JWT)
    [Authorize]
    [HttpGet("protected")]
    public IActionResult Protected()
    {
        return Ok("You accessed a protected endpoint!");
    }

    // Admin-only endpoint
    [Authorize(Roles = "Admin")]
    [HttpGet("admin-only")]
    public IActionResult AdminOnly()
    {
        return Ok("You are an admin!");
    }

    private readonly AuthService _authService;
    private readonly IConfiguration _config;

    public AuthController(AuthService authService, IConfiguration config)
    {
        _authService = authService;
        _config = config;
    }

    [HttpPost("register")]
    public IActionResult Register([FromBody] RegisterDto dto)
    {
        var role = string.IsNullOrEmpty(dto.Role) ? "User" : dto.Role;
        var result = _authService.Register(dto.Email, dto.Password, dto.Name, role);

        if (!result)
            return BadRequest("User already exists.");

        return Ok($"Registration successful with role {role}.");
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginDto dto)
    {
        var user = _authService.ValidateUser(dto.Email, dto.Password);
        if (user == null)
            return Unauthorized("Invalid credentials.");

        // JWT generation here
        var claims = new[]
{
    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
    new Claim(ClaimTypes.Email, user.Email),
    new Claim(ClaimTypes.Name, user.Name ?? ""),
    new Claim(ClaimTypes.Role, user.Role) // New claim
};

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds
        );

        return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
    }
}
