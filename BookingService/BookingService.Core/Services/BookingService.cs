using BookingService.Core.Dtos;
using BookingService.Core.Entities;
using BookingService.Core.Interfaces;

namespace BookingService.Core.Services;

public class BookingService : IBookingService
{
    private readonly IBookingRepository _bookingRepository;
    // TODO: Use HTTP clients to communicate with RoomService and NotificationService
    // private readonly IRoomService _roomClient;
    // private readonly INotificationService _notificationClient;

    public BookingService(IBookingRepository bookingRepository)
    {
        _bookingRepository = bookingRepository;
    }

    public async Task<ServiceResult<BookingDto>> CreateBookingAsync(int userId, CreateBookingDto createBookingDto)
    {
        // TODO: Validate room exists and is available via RoomService
        // var room = await _roomClient.GetRoomByIdAsync(createBookingDto.RoomId);
        // if (room == null) return ServiceResult<BookingDto>.Failure("Room not found.");
        // if (!room.IsAvailable) return ServiceResult<BookingDto>.Failure("Room is not available.");

        var isOverlapping = await _bookingRepository.AnyOverlappingAsync(
            createBookingDto.RoomId, 
            createBookingDto.CheckInDate, 
            createBookingDto.CheckOutDate);
            
        if (isOverlapping) return ServiceResult<BookingDto>.Failure("Room is already booked for the selected dates.");

        var booking = new Booking
        {
            RoomId = createBookingDto.RoomId,
            UserId = userId,
            CheckInDate = createBookingDto.CheckInDate,
            CheckOutDate = createBookingDto.CheckOutDate,
            IsCancelled = false
        };

        await _bookingRepository.AddAsync(booking);
        
        // TODO: Notify user via NotificationService
        // await _notificationClient.NotifyAsync(userId, $"Your booking has been confirmed.");

        return ServiceResult<BookingDto>.SuccessResult(new BookingDto(booking.Id, booking.RoomId, booking.UserId, booking.CheckInDate, booking.CheckOutDate, booking.IsCancelled));
    }

    public async Task<bool> CancelBookingAsync(int bookingId, int userId)
    {
        var booking = await _bookingRepository.GetByIdAsync(bookingId);
        if (booking == null || booking.UserId != userId || booking.IsCancelled) return false;

        booking.IsCancelled = true;
        await _bookingRepository.UpdateAsync(booking);

        // TODO: Notify user via NotificationService
        // await _notificationClient.NotifyAsync(userId, $"Your booking {bookingId} has been cancelled.");

        return true;
    }

    public async Task<IEnumerable<BookingDto>> GetAllBookingsAsync()
    {
        var bookings = await _bookingRepository.GetAllAsync();
        return bookings.Select(b => new BookingDto(b.Id, b.RoomId, b.UserId, b.CheckInDate, b.CheckOutDate, b.IsCancelled));
    }
}
