using UserService.Core.Dtos;
using UserService.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace UserService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;

    public AuthController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto registerDto)
    {
        var result = await _userService.RegisterAsync(registerDto);
        if (result == null) return BadRequest("Username already exists");
        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        var result = await _userService.LoginAsync(loginDto);
        if (result == null) return Unauthorized("Invalid username or password");
        return Ok(result);
    }
}
