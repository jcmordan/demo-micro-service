using RoomService.Core.Entities;
using Microsoft.EntityFrameworkCore;
using RoomService.Core.Interfaces;
using RoomService.Infrastructure.Data;

namespace RoomService.Infrastructure.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        private readonly RoomDbContext _context;

        public RoomRepository(RoomDbContext context)
        {
            _context = context;
        }

        public async Task<Room?> GetByIdAsync(int id) => await _context.Rooms.FindAsync(id);

        public async Task<IEnumerable<Room>> GetAllAsync() => await _context.Rooms.ToListAsync();

        public async Task AddAsync(Room room)
        {
            await _context.Rooms.AddAsync(room);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Room room)
        {
            _context.Rooms.Update(room);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room != null)
            {
                _context.Rooms.Remove(room);
                await _context.SaveChangesAsync();
            }
        }
    }
}
