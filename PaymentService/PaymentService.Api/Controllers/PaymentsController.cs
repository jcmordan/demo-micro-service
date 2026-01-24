using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaymentService.Core.Dtos;
using PaymentService.Core.Services;
using Microsoft.Extensions.Logging;

namespace PaymentService.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PaymentsController : ControllerBase
{
    private readonly PaymentsService _paymentService;
    private readonly ILogger<PaymentsController> _logger;

    public PaymentsController(PaymentsService paymentService, ILogger<PaymentsController> logger)
    {
        _paymentService = paymentService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<ActionResult<PaymentDto>> AddPayment(AddPaymentDto addDto)
    {
        _logger.LogInformation("[AddPayment] Request received for Booking: {BookingId}, Amount: {Amount}", addDto.BookingId, addDto.Amount);
        var result = await _paymentService.AddPaymentAsync(addDto);
        if (result == null)
        {
            _logger.LogWarning("[AddPayment] Payment processing failed for Booking: {BookingId}", addDto.BookingId);
            return BadRequest("Invalid payment request or booking not found.");
        }

        return Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PaymentDto>>> GetPayments([FromQuery] string? status)
    {
        _logger.LogInformation("[GetPayments] Request received with status filter: {Status}", status ?? "All");
        var payments = await _paymentService.GetPaymentsAsync(status);
        return Ok(payments);
    }
}
