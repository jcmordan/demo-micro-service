using System.Security.Claims;
using BookingService.Core.Dtos;
using BookingService.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookingService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BookingsController : ControllerBase
{
    private readonly IBookingService _bookingService;

    public BookingsController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var bookings = await _bookingService.GetAllBookingsAsync();
        return Ok(bookings);
    }

    [HttpPost]
    public async Task<IActionResult> Book(CreateBookingDto createBookingDto)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        var result = await _bookingService.CreateBookingAsync(userId, createBookingDto);
        if (!result.Success) return BadRequest(result.ErrorMessage);
        return Ok(result.Data);
    }

    [HttpPost("{id}/cancel")]
    public async Task<IActionResult> Cancel(int id)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        var result = await _bookingService.CancelBookingAsync(id, userId);
        if (!result) return BadRequest("Could not cancel booking");
        return Ok("Booking cancelled");
    }
}
