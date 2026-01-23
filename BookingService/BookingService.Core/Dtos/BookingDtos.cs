using System;

namespace BookingService.Core.Dtos;

public record CreateBookingDto(int RoomId, DateTime CheckInDate, DateTime CheckOutDate);

public record BookingDto(int Id, int RoomId, int UserId, DateTime CheckInDate, DateTime CheckOutDate, bool IsCancelled);
