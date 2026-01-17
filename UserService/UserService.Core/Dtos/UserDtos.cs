namespace UserService.Core.Dtos;

public record UserDto(int Id, string Username, string Email);
public record RegisterDto(string Username, string Email, string Password);
public record LoginDto(string Username, string Password);
public record AuthResponseDto(string Token, UserDto User);