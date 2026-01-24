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

namespace BookingService.Core.Services;

public class OrderBookingService
{
    private readonly IBookingRepository _bookingRepository;
    private readonly HttpClient _httpClient;
    private readonly ServiceOptions _serviceOptions;

    public OrderBookingService(
        IBookingRepository bookingRepository, 
        HttpClient httpClient, 
        IOptions<ServiceOptions> serviceOptions)
    {
        _bookingRepository = bookingRepository;
        _httpClient = httpClient;
        _serviceOptions = serviceOptions.Value;
    }

    public async Task<BookingDto?> CreateBookingAsync(int userId, CreateBookingDto createDto)
    {
        // 1. Check if Room exists in RoomService via HTTP
        var roomServiceBaseUrl = _serviceOptions.ServiceUrls.GetValueOrDefault("RoomService", "http://localhost:5071");
        var roomServiceUrl = $"{roomServiceBaseUrl}/api/rooms/{createDto.RoomId}";
        
        try
        {
            var roomResponse = await _httpClient.GetAsync(roomServiceUrl);
            if (!roomResponse.IsSuccessStatusCode)
            {
                return null; // Room not found or error
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null; // Interface call failed
        }

        // 2. Check for overlapping bookings
        var isOverlapping = await _bookingRepository.AnyOverlappingAsync(
            createDto.RoomId, 
            createDto.CheckInDate, 
            createDto.CheckOutDate);
            
        if (isOverlapping)
        {
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
        var booking = await _bookingRepository.GetByIdAsync(id);
        
        if (booking == null || booking.UserId != userId || booking.IsCancelled)
        {
            return false;
        }

        booking.IsCancelled = true;
        await _bookingRepository.UpdateAsync(booking);
        
        return true;
    }

    public async Task<BookingDto?> GetBookingByIdAsync(int id)
    {
        var booking = await _bookingRepository.GetByIdAsync(id);
        
        if (booking == null)
        {
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
