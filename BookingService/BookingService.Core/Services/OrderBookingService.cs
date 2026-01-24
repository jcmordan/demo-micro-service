using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BookingService.Core.Dtos;
using BookingService.Core.Entities;
using BookingService.Core.Interfaces;
using BookingApp.Common.Options;
using Microsoft.Extensions.Options;

using Microsoft.Extensions.Logging;

namespace BookingService.Core.Services;

public class OrderBookingService
{
    private readonly IBookingRepository _bookingRepository;
    private readonly HttpClient _httpClient;
    private readonly ServiceOptions _serviceOptions;
    private readonly ILogger<OrderBookingService> _logger;

    public OrderBookingService(
        IBookingRepository bookingRepository, 
        HttpClient httpClient, 
        IOptions<ServiceOptions> serviceOptions,
        ILogger<OrderBookingService> logger)
    {
        _bookingRepository = bookingRepository;
        _httpClient = httpClient;
        _serviceOptions = serviceOptions.Value;
        _logger = logger;
    }

    public async Task<BookingDto?> CreateBookingAsync(int userId, CreateBookingDto createDto)
    {
        _logger.LogInformation("[CreateBookingAsync] Attempting to create booking for Room: {RoomId}, User: {UserId}", createDto.RoomId, userId);
        
        // 1. Check if Room exists in RoomService via HTTP
        var roomServiceBaseUrl = _serviceOptions.ServiceUrls.GetValueOrDefault("RoomService", "http://localhost:5071");
        var roomServiceUrl = $"{roomServiceBaseUrl}/api/rooms/{createDto.RoomId}";
        
        try
        {
            _logger.LogInformation("[CreateBookingAsync] Calling RoomService at: {Url}", roomServiceUrl);
            var roomResponse = await _httpClient.GetAsync(roomServiceUrl);
            if (!roomResponse.IsSuccessStatusCode)
            {
                _logger.LogWarning("[CreateBookingAsync] Room not found or error from RoomService: {StatusCode}", roomResponse.StatusCode);
                return null; // Room not found or error
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[CreateBookingAsync] Error calling RoomService");
            return null; // Interface call failed
        }

        // 2. Check for overlapping bookings
        var isOverlapping = await _bookingRepository.AnyOverlappingAsync(
            createDto.RoomId, 
            createDto.CheckInDate, 
            createDto.CheckOutDate);
            
        if (isOverlapping)
        {
            _logger.LogWarning("[CreateBookingAsync] Overlapping booking detected for Room: {RoomId}", createDto.RoomId);
            return null; // Biz error: overlap
        }

        var booking = new Booking
        {
            RoomId = createDto.RoomId,
            UserId = userId,
            CheckInDate = createDto.CheckInDate,
            CheckOutDate = createDto.CheckOutDate,
            IsCancelled = false
        };

        await _bookingRepository.AddAsync(booking);

        _logger.LogInformation("[CreateBookingAsync] Successfully created booking ID: {BookingId}", booking.Id);

        return new BookingDto(
            booking.Id, 
            booking.RoomId, 
            booking.UserId, 
            booking.CheckInDate, 
            booking.CheckOutDate, 
            booking.IsCancelled);
    }

    public async Task<IEnumerable<BookingDto>> GetAllBookingsAsync()
    {
        var bookings = await _bookingRepository.GetAllAsync();
        var dtos = new List<BookingDto>();
        
        foreach (var b in bookings)
        {
            dtos.Add(new BookingDto(b.Id, b.RoomId, b.UserId, b.CheckInDate, b.CheckOutDate, b.IsCancelled));
        }
        
        return dtos;
    }

    public async Task<IEnumerable<BookingDto>> GetUserBookingsAsync(int userId)
    {
        var bookings = await _bookingRepository.GetByUserIdAsync(userId);
        var dtos = new List<BookingDto>();
        
        foreach (var b in bookings)
        {
            dtos.Add(new BookingDto(b.Id, b.RoomId, b.UserId, b.CheckInDate, b.CheckOutDate, b.IsCancelled));
        }
        
        return dtos;
    }

    public async Task<bool> CancelBookingAsync(int id, int userId)
    {
        _logger.LogInformation("[CancelBookingAsync] Attempting to cancel booking ID: {BookingId} for User: {UserId}", id, userId);
        
        var booking = await _bookingRepository.GetByIdAsync(id);
        
        if (booking == null || booking.UserId != userId || booking.IsCancelled)
        {
            _logger.LogWarning("[CancelBookingAsync] Cancellation failed for booking ID: {BookingId}", id);
            return false;
        }

        booking.IsCancelled = true;
        await _bookingRepository.UpdateAsync(booking);
        
        _logger.LogInformation("[CancelBookingAsync] Successfully cancelled booking ID: {BookingId}", id);
        
        return true;
    }

    public async Task<BookingDto?> GetBookingByIdAsync(int id)
    {
        _logger.LogInformation("[GetBookingByIdAsync] Retrieving booking ID: {BookingId}", id);
        var booking = await _bookingRepository.GetByIdAsync(id);
        
        if (booking == null)
        {
            _logger.LogWarning("[GetBookingByIdAsync] Booking ID: {BookingId} not found", id);
            return null;
        }

        return new BookingDto(
            booking.Id, 
            booking.RoomId, 
            booking.UserId, 
            booking.CheckInDate, 
            booking.CheckOutDate, 
            booking.IsCancelled);
    }
}
