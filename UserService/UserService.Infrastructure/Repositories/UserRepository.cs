using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UserService.Core.Entities;
using UserService.Core.Interfaces;
using UserService.Infrastructure.Data;

namespace UserService.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserDbContext _context;
    private readonly ILogger<UserRepository> _logger;

    public UserRepository(UserDbContext context, ILogger<UserRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        _logger.LogInformation("[GetByIdAsync] Fetching user ID: {UserId}", id);
        return await _context.Users.FindAsync(id);
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        _logger.LogInformation("[GetByUsernameAsync] Fetching user by username: {Username}", username);
        return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        _logger.LogInformation("[GetByEmailAsync] Fetching user by email: {Email}", email);
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        _logger.LogInformation("[GetAllAsync] Fetching all users");
        return await _context.Users.ToListAsync();
    }

    public async Task AddAsync(User user)
    {
        _logger.LogInformation("[AddAsync] Adding new user: {Username}", user.Username);
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }
}
