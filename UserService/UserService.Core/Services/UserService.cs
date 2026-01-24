using Microsoft.Extensions.Logging;
using UserService.Core.Dtos;
using UserService.Core.Entities;
using UserService.Core.Interfaces;

namespace UserService.Core.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtService _jwtService;
    private readonly ILogger<UserService> _logger;

    public UserService(
        IUserRepository userRepository, 
        IPasswordHasher passwordHasher, 
        IJwtService jwtService,
        ILogger<UserService> logger)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtService = jwtService;
        _logger = logger;
    }

    public async Task<AuthResponseDto?> RegisterAsync(RegisterDto registerDto)
    {
        _logger.LogInformation("[RegisterAsync] Attempting to register user: {Username}", registerDto.Username);
        if (await _userRepository.GetByUsernameAsync(registerDto.Username) != null)
        {
            _logger.LogWarning("[RegisterAsync] User registration failed. Username {Username} already exists", registerDto.Username);
            return null;
        }

        var user = new User
        {
            Username = registerDto.Username,
            Email = registerDto.Email,
            PasswordHash = _passwordHasher.HashPassword(registerDto.Password)
        };

        await _userRepository.AddAsync(user);
        _logger.LogInformation("[RegisterAsync] User registered successfully: {Username}", registerDto.Username);

        return new AuthResponseDto(_jwtService.GenerateToken(user), new UserDto(user.Id, user.Username, user.Email));
    }

    public async Task<AuthResponseDto?> LoginAsync(LoginDto loginDto)
    {
        _logger.LogInformation("[LoginAsync] Attempting login for user: {Username}", loginDto.Username);
        var user = await _userRepository.GetByUsernameAsync(loginDto.Username);
        
        if (user == null || !_passwordHasher.VerifyPassword(loginDto.Password, user.PasswordHash))
        {
            _logger.LogWarning("[LoginAsync] Login failed for user: {Username}", loginDto.Username);
            return null;
        }

        _logger.LogInformation("[LoginAsync] User logged in successfully: {Username}", loginDto.Username);
        return new AuthResponseDto(_jwtService.GenerateToken(user), new UserDto(user.Id, user.Username, user.Email));
    }

    public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
    {
        _logger.LogInformation("[GetAllUsersAsync] Fetching all users");
        var users = await _userRepository.GetAllAsync();
        return users.Select(u => new UserDto(u.Id, u.Username, u.Email));
    }

    public async Task<UserDto?> GetUserByIdAsync(int id)
    {
        _logger.LogInformation("[GetUserByIdAsync] Fetching user by ID: {UserId}", id);
        var user = await _userRepository.GetByIdAsync(id);
        
        if (user == null)
        {
            _logger.LogWarning("[GetUserByIdAsync] User ID: {UserId} not found", id);
            return null;
        }

        return new UserDto(user.Id, user.Username, user.Email);
    }
}
