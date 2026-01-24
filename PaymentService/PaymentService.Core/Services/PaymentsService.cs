using System.Net.Http.Json;
using Microsoft.Extensions.Options;
using PaymentService.Core.Dtos;
using PaymentService.Core.Entities;
using PaymentService.Core.Interfaces;
using BookingApp.Common.Options;

namespace PaymentService.Core.Services;

public class PaymentsService
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly HttpClient _httpClient;
    private readonly ServiceOptions _serviceOptions;

    public PaymentsService(
        IPaymentRepository paymentRepository, 
        HttpClient httpClient, 
        IOptions<ServiceOptions> serviceOptions)
    {
        _paymentRepository = paymentRepository;
        _httpClient = httpClient;
        _serviceOptions = serviceOptions.Value;
    }

    public async Task<PaymentDto?> AddPaymentAsync(AddPaymentDto addDto)
    {
        // 1. Validate booking exists in BookingService via HTTP
        var bookingServiceBaseUrl = _serviceOptions.ServiceUrls.GetValueOrDefault("BookingService", "http://localhost:5072");
        var bookingServiceUrl = $"{bookingServiceBaseUrl}/api/bookings/{addDto.BookingId}";

        try
        {
            var response = await _httpClient.GetAsync(bookingServiceUrl);
            if (!response.IsSuccessStatusCode)
            {
                return null; // Booking not found or error
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error validating booking: {ex.Message}");
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

        return new PaymentDto(
            payment.Id,
            payment.BookingId,
            payment.Amount,
            payment.PaymentDate,
            payment.Status);
    }

    public async Task<IEnumerable<PaymentDto>> GetPaymentsAsync(string? status)
    {
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
