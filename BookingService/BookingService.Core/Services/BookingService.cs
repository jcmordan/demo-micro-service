using BookingService.Core.Dtos;
using BookingService.Core.Entities;
using BookingService.Core.Interfaces;

namespace BookingService.Core.Services;

public class BookingService : IBookingService
{
    private readonly IBookingRepository _bookingRepository;
    // En microservicios, estos se llamarían a través de otros servicios.
    // Para simplificar el ejercicio, asumiremos que existen.
    
    public BookingService(IBookingRepository bookingRepository)
    {
        _bookingRepository = bookingRepository;
    }

    public async Task<ServiceResult<BookingDto>> CreateBookingAsync(int userId, CreateBookingDto createBookingDto)
    {
        // En una implementación real, aquí llamaríamos al RoomService por HTTP
        // Para este ejercicio, validamos que el RoomId sea válido (>0)
        if (createBookingDto.RoomId <= 0) 
            return ServiceResult<BookingDto>.Failure("Habitación no encontrada.");

        var isOverlapping = await _bookingRepository.AnyOverlappingAsync(
            createBookingDto.RoomId, 
            createBookingDto.CheckInDate, 
            createBookingDto.CheckOutDate);
            
        if (isOverlapping) 
            return ServiceResult<BookingDto>.Failure("La habitación ya está reservada para las fechas seleccionadas.");

        var booking = new Booking
        {
            RoomId = createBookingDto.RoomId,
            UserId = userId,
            CheckInDate = createBookingDto.CheckInDate,
            CheckOutDate = createBookingDto.CheckOutDate,
            IsCancelled = false
        };

        await _bookingRepository.AddAsync(booking);
        
        return ServiceResult<BookingDto>.SuccessResult(new BookingDto(
            booking.Id, 
            booking.RoomId, 
            booking.UserId, 
            booking.CheckInDate, 
            booking.CheckOutDate, 
            booking.IsCancelled));
    }

    public async Task<bool> CancelBookingAsync(int bookingId, int userId)
    {
        var booking = await _bookingRepository.GetByIdAsync(bookingId);
        if (booking == null || booking.UserId != userId || booking.IsCancelled) return false;

        booking.IsCancelled = true;
        await _bookingRepository.UpdateAsync(booking);

        return true;
    }

    public async Task<IEnumerable<BookingDto>> GetAllBookingsAsync()
    {
        var bookings = await _bookingRepository.GetAllAsync();
        return bookings.Select(b => new BookingDto(b.Id, b.RoomId, b.UserId, b.CheckInDate, b.CheckOutDate, b.IsCancelled));
    }
}
