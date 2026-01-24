using BookingService.Core.Entities;
using BookingService.Core.Interfaces;
using BookingService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BookingService.Infrastructure.Repositories;

public class BookingRepository : IBookingRepository
{
    private readonly BookingDbContext _context;

    public BookingRepository(BookingDbContext context)
    {
        _context = context;
    }

    public async Task<Booking?> GetByIdAsync(int id) => await _context.Bookings.FindAsync(id);

    public async Task<IEnumerable<Booking>> GetAllAsync() => await _context.Bookings.ToListAsync();

    public async Task<IEnumerable<Booking>> GetByUserIdAsync(int userId) => 
        await _context.Bookings.Where(b => b.UserId == userId).ToListAsync();

    public async Task AddAsync(Booking booking)
    {
        await _context.Bookings.AddAsync(booking);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Booking booking)
    {
         _context.Bookings.Update(booking);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> AnyOverlappingAsync(int roomId, DateTime checkIn, DateTime checkOut)
    {
        return await _context.Bookings.AnyAsync(b => 
            b.RoomId == roomId && 
            !b.IsCancelled &&
            ((checkIn >= b.CheckInDate && checkIn < b.CheckOutDate) || 
             (checkOut > b.CheckInDate && checkOut <= b.CheckOutDate) ||
             (checkIn <= b.CheckInDate && checkOut >= b.CheckOutDate)));
    }
}
