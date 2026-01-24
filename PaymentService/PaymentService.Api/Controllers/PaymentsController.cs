using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaymentService.Core.Dtos;
using PaymentService.Core.Services;

namespace PaymentService.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PaymentsController : ControllerBase
{
    private readonly PaymentsService _paymentService;

    public PaymentsController(PaymentsService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpPost]
    public async Task<ActionResult<PaymentDto>> AddPayment(AddPaymentDto addDto)
    {
        var result = await _paymentService.AddPaymentAsync(addDto);
        if (result == null)
        {
            return BadRequest("Invalid payment request or booking not found.");
        }

        return Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PaymentDto>>> GetPayments([FromQuery] string? status)
    {
        var payments = await _paymentService.GetPaymentsAsync(status);
        return Ok(payments);
    }
}
