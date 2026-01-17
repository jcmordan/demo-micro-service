using UserService.Core.Dtos;
using UserService.Core.Entities;
using UserService.Core.Interfaces;

namespace UserService.Core.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtService _jwtService;

    public UserService(IUserRepository userRepository, IPasswordHasher passwordHasher, IJwtService jwtService)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtService = jwtService;
    }

    public async Task<AuthResponseDto?> RegisterAsync(RegisterDto registerDto)
    {
        if (await _userRepository.GetByUsernameAsync(registerDto.Username) != null) return null;

        var user = new User
        {
            Username = registerDto.Username,
            Email = registerDto.Email,
            PasswordHash = _passwordHasher.HashPassword(registerDto.Password)
        };

        await _userRepository.AddAsync(user);

        return new AuthResponseDto(_jwtService.GenerateToken(user), new UserDto(user.Id, user.Username, user.Email));
    }

    public async Task<AuthResponseDto?> LoginAsync(LoginDto loginDto)
    {
        var user = await _userRepository.GetByUsernameAsync(loginDto.Username);
        if (user == null || !_passwordHasher.VerifyPassword(loginDto.Password, user.PasswordHash)) return null;

        return new AuthResponseDto(_jwtService.GenerateToken(user), new UserDto(user.Id, user.Username, user.Email));
    }

    public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
    {
        var users = await _userRepository.GetAllAsync();
        return users.Select(u => new UserDto(u.Id, u.Username, u.Email));
    }

    public async Task<UserDto?> GetUserByIdAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        return user == null ? null : new UserDto(user.Id, user.Username, user.Email);
    }
}
