using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BookingService.Core.Entities;
using BookingService.Core.Interfaces;
using BookingService.Infrastructure.Data;

using Microsoft.Extensions.Logging;

namespace BookingService.Infrastructure.Repositories;

public class BookingRepository : IBookingRepository
{
    private readonly BookingDbContext _context;
    private readonly ILogger<BookingRepository> _logger;

    public BookingRepository(BookingDbContext context, ILogger<BookingRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Booking?> GetByIdAsync(int id)
    {
        _logger.LogInformation("[GetByIdAsync] Fetching booking with ID: {BookingId}", id);
        return await _context.Bookings.FindAsync(id);
    }

    public async Task<IEnumerable<Booking>> GetAllAsync()
    {
        _logger.LogInformation("[GetAllAsync] Fetching all bookings");
        return await _context.Bookings.ToListAsync();
    }

    public async Task<IEnumerable<Booking>> GetByUserIdAsync(int userId)
    {
        _logger.LogInformation("[GetByUserIdAsync] Fetching bookings for User ID: {UserId}", userId);
        return await _context.Bookings.Where(b => b.UserId == userId).ToListAsync();
    }

    public async Task AddAsync(Booking booking)
    {
        _logger.LogInformation("[AddAsync] Adding new booking for Room: {RoomId}, User: {UserId}", booking.RoomId, booking.UserId);
        await _context.Bookings.AddAsync(booking);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Booking booking)
    {
        _logger.LogInformation("[UpdateAsync] Updating booking ID: {BookingId}", booking.Id);
        _context.Bookings.Update(booking);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> AnyOverlappingAsync(int roomId, DateTime checkIn, DateTime checkOut)
    {
        _logger.LogInformation("[AnyOverlappingAsync] Checking overlap for Room: {RoomId}, Date: {CheckIn} to {CheckOut}", roomId, checkIn, checkOut);
        return await _context.Bookings.AnyAsync(b => 
            b.RoomId == roomId && 
            !b.IsCancelled &&
            ((checkIn >= b.CheckInDate && checkIn < b.CheckOutDate) ||
             (checkOut > b.CheckInDate && checkOut <= b.CheckOutDate) ||
             (checkIn <= b.CheckInDate && checkOut >= b.CheckOutDate)));
    }
}
