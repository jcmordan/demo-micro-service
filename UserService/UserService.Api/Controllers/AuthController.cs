using UserService.Core.Dtos;
using UserService.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace UserService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IUserService userService, ILogger<AuthController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto registerDto)
    {
        _logger.LogInformation("[Register] Received registration request for user: {Username}", registerDto.Username);
        var result = await _userService.RegisterAsync(registerDto);
        if (result == null) 
        {
            _logger.LogWarning("[Register] Registration failed for user: {Username}", registerDto.Username);
            return BadRequest("Username already exists");
        }
        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        _logger.LogInformation("[Login] Received login request for user: {Username}", loginDto.Username);
        var result = await _userService.LoginAsync(loginDto);
        if (result == null) 
        {
            _logger.LogWarning("[Login] Login failed for user: {Username}", loginDto.Username);
            return Unauthorized("Invalid username or password");
        }
        return Ok(result);
    }
}
