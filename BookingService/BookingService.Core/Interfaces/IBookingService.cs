using BookingService.Core.Dtos;

namespace BookingService.Core.Interfaces;

public interface IBookingService
{
    Task<ServiceResult<BookingDto>> CreateBookingAsync(int userId, CreateBookingDto createBookingDto);
    Task<bool> CancelBookingAsync(int bookingId, int userId);
    Task<IEnumerable<BookingDto>> GetAllBookingsAsync();
}
