using System.Collections.Generic;
using System.Threading.Tasks;
using BookingService.Core.Entities;

namespace BookingService.Core.Interfaces;

public interface IBookingRepository
{
    Task<Booking?> GetByIdAsync(int id);
    Task<IEnumerable<Booking>> GetAllAsync();
    Task<IEnumerable<Booking>> GetByUserIdAsync(int userId);
    Task AddAsync(Booking booking);
    Task UpdateAsync(Booking booking);
    Task<bool> AnyOverlappingAsync(int roomId, DateTime checkIn, DateTime checkOut);
}
