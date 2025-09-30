using FoodDelivery.BusinessLogic.DTOs;
using FoodDelivery.BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public IActionResult Register([FromBody] RegisterDto dto)
    {
        var result = _authService.Register(dto.Email, dto.Password, dto.Name);
        if (!result)
            return BadRequest("User already exists.");

        return Ok("Registration successful.");
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginDto dto)
    {
        var user = _authService.ValidateUser(dto.Email, dto.Password);
        if (user == null)
            return Unauthorized("Invalid credentials.");

        return Ok("Login successful (JWT will be added next).");
    }
}
