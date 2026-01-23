using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BookingService.Core.Dtos;
using BookingService.Core.Services;

namespace BookingService.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class BookingsController : ControllerBase
{
    private readonly OrderBookingService _bookingService;

    public BookingsController(OrderBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookingDto>>> GetAll()
    {
        var bookings = await _bookingService.GetAllBookingsAsync();
        return Ok(bookings);
    }

    [HttpGet("my-bookings")]
    public async Task<ActionResult<IEnumerable<BookingDto>>> GetMyBookings()
    {
        var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!int.TryParse(userIdStr, out var userId))
        {
            return Unauthorized();
        }

        var bookings = await _bookingService.GetUserBookingsAsync(userId);
        return Ok(bookings);
    }

    [HttpPost]
    public async Task<ActionResult<BookingDto>> Create(CreateBookingDto createDto)
    {
        var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!int.TryParse(userIdStr, out var userId))
        {
            return Unauthorized();
        }

        var booking = await _bookingService.CreateBookingAsync(userId, createDto);
        
        if (booking == null)
        {
            return BadRequest("Could not create booking. Room may not exist or is already booked for these dates.");
        }

        return CreatedAtAction(nameof(GetAll), new { id = booking.Id }, booking);
    }

    [HttpPost("{id}/cancel")]
    public async Task<IActionResult> Cancel(int id)
    {
        var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!int.TryParse(userIdStr, out var userId))
        {
            return Unauthorized();
        }

        var success = await _bookingService.CancelBookingAsync(id, userId);
        
        if (!success)
        {
            return BadRequest("Could not cancel booking.");
        }

        return Ok("Booking cancelled successfully.");
    }
}
