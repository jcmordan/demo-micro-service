using System.Net.Http.Json;
using Microsoft.Extensions.Options;
using PaymentService.Core.Dtos;
using PaymentService.Core.Entities;
using PaymentService.Core.Interfaces;
using BookingApp.Common.Options;

using Microsoft.Extensions.Logging;

namespace PaymentService.Core.Services;

public class PaymentsService
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly HttpClient _httpClient;
    private readonly ServiceOptions _serviceOptions;
    private readonly ILogger<PaymentsService> _logger;

    public PaymentsService(
        IPaymentRepository paymentRepository, 
        HttpClient httpClient, 
        IOptions<ServiceOptions> serviceOptions,
        ILogger<PaymentsService> logger)
    {
        _paymentRepository = paymentRepository;
        _httpClient = httpClient;
        _serviceOptions = serviceOptions.Value;
        _logger = logger;
    }

    public async Task<PaymentDto?> AddPaymentAsync(AddPaymentDto addDto)
    {
        _logger.LogInformation("[AddPaymentAsync] Attempting to add payment for Booking ID: {BookingId}, Amount: {Amount}", addDto.BookingId, addDto.Amount);
        
        // 1. Validate booking exists in BookingService via HTTP
        var bookingServiceBaseUrl = _serviceOptions.ServiceUrls.GetValueOrDefault("BookingService", "http://localhost:5072");
        var bookingServiceUrl = $"{bookingServiceBaseUrl}/api/bookings/{addDto.BookingId}";

        try
        {
            _logger.LogInformation("[AddPaymentAsync] Validating booking at: {Url}", bookingServiceUrl);
            var response = await _httpClient.GetAsync(bookingServiceUrl);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("[AddPaymentAsync] Booking validation failed for ID: {BookingId}. Status: {StatusCode}", addDto.BookingId, response.StatusCode);
                return null; // Booking not found or error
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[AddPaymentAsync] Error calling BookingService");
            return null; // Interface call failed
        }

        // 2. Create and save payment
        var payment = new Payment
        {
            BookingId = addDto.BookingId,
            Amount = addDto.Amount,
            PaymentDate = DateTime.UtcNow,
            Status = "Completed"
        };

        await _paymentRepository.AddAsync(payment);

        _logger.LogInformation("[AddPaymentAsync] Successfully processed payment ID: {PaymentId}", payment.Id);

        return new PaymentDto(
            payment.Id,
            payment.BookingId,
            payment.Amount,
            payment.PaymentDate,
            payment.Status);
    }

    public async Task<IEnumerable<PaymentDto>> GetPaymentsAsync(string? status)
    {
        _logger.LogInformation("[GetPaymentsAsync] Retrieving payments with filter status: {Status}", status ?? "None");
        IEnumerable<Payment> payments;
        
        if (string.IsNullOrEmpty(status))
        {
            payments = await _paymentRepository.GetAllAsync();
        }
        else
        {
            payments = await _paymentRepository.GetByStatusAsync(status);
        }

        return payments.Select(p => new PaymentDto(
            p.Id,
            p.BookingId,
            p.Amount,
            p.PaymentDate,
            p.Status));
    }
}
