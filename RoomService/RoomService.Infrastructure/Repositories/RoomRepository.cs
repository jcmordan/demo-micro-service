using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RoomService.Core.Entities;
using RoomService.Core.Interfaces;
using RoomService.Infrastructure.Data;

namespace RoomService.Infrastructure.Repositories;

public class RoomRepository : IRoomRepository
{
    private readonly RoomDbContext _context;
    private readonly ILogger<RoomRepository> _logger;

    public RoomRepository(RoomDbContext context, ILogger<RoomRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Room?> GetByIdAsync(int id)
    {
        _logger.LogInformation("[GetByIdAsync] Fetching room ID: {RoomId}", id);
        return await _context.Rooms.FindAsync(id);
    }

    public async Task<IEnumerable<Room>> GetAllAsync()
    {
        _logger.LogInformation("[GetAllAsync] Fetching all rooms");
        return await _context.Rooms.ToListAsync();
    }

    public async Task AddAsync(Room room)
    {
        _logger.LogInformation("[AddAsync] Adding new room: {RoomName}", room.Name);
        await _context.Rooms.AddAsync(room);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Room room)
    {
        _logger.LogInformation("[UpdateAsync] Updating room ID: {RoomId}", room.Id);
        _context.Rooms.Update(room);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        _logger.LogInformation("[DeleteAsync] Deleting room ID: {RoomId}", id);
        var room = await _context.Rooms.FindAsync(id);
        
        if (room != null)
        {
            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();
            _logger.LogInformation("[DeleteAsync] Successfully deleted room ID: {RoomId}", id);
        }
        else
        {
            _logger.LogWarning("[DeleteAsync] Room ID: {RoomId} not found for deletion", id);
        }
    }
}
