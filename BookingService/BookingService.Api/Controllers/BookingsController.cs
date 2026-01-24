using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BookingService.Core.Dtos;
using BookingService.Core.Services;
using Microsoft.Extensions.Logging;

namespace BookingService.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class BookingsController : ControllerBase
{
    private readonly OrderBookingService _bookingService;
    private readonly ILogger<BookingsController> _logger;

    public BookingsController(OrderBookingService bookingService, ILogger<BookingsController> logger)
    {
        _bookingService = bookingService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookingDto>>> GetAll()
    {
        _logger.LogInformation("[GetAll] Received request to fetch all bookings");
        var bookings = await _bookingService.GetAllBookingsAsync();
        return Ok(bookings);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BookingDto>> GetById(int id)
    {
        _logger.LogInformation("[GetById] Fetching booking ID: {BookingId}", id);
        var booking = await _bookingService.GetBookingByIdAsync(id);

        if (booking == null)
        {
            _logger.LogWarning("[GetById] Booking ID: {BookingId} not found", id);
            return NotFound();
        }

        return Ok(booking);
    }

    [HttpGet("my-bookings")]
    public async Task<ActionResult<IEnumerable<BookingDto>>> GetMyBookings()
    {
        _logger.LogInformation("[GetMyBookings] Fetching bookings for current user");
        var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!int.TryParse(userIdStr, out var userId))
        {
            _logger.LogWarning("[GetMyBookings] User unauthorized");
            return Unauthorized();
        }

        var bookings = await _bookingService.GetUserBookingsAsync(userId);
        return Ok(bookings);
    }

    [HttpPost]
    public async Task<ActionResult<BookingDto>> Create(CreateBookingDto createDto)
    {
        _logger.LogInformation("[Create] Request to create booking for Room: {RoomId}", createDto.RoomId);
        var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!int.TryParse(userIdStr, out var userId))
        {
            _logger.LogWarning("[Create] User unauthorized");
            return Unauthorized();
        }

        var booking = await _bookingService.CreateBookingAsync(userId, createDto);
        
        if (booking == null)
        {
            _logger.LogWarning("[Create] Failed to create booking for Room: {RoomId}", createDto.RoomId);
            return BadRequest("Could not create booking. Room may not exist or is already booked for these dates.");
        }

        return CreatedAtAction(nameof(GetAll), new { id = booking.Id }, booking);
    }

    [HttpPost("{id}/cancel")]
    public async Task<IActionResult> Cancel(int id)
    {
        _logger.LogInformation("[Cancel] Request to cancel booking ID: {BookingId}", id);
        var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!int.TryParse(userIdStr, out var userId))
        {
            _logger.LogWarning("[Cancel] User unauthorized");
            return Unauthorized();
        }

        var success = await _bookingService.CancelBookingAsync(id, userId);
        
        if (!success)
        {
            _logger.LogWarning("[Cancel] Failed to cancel booking ID: {BookingId}", id);
            return BadRequest("Could not cancel booking.");
        }

        return Ok("Booking cancelled successfully.");
    }
}
