using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PaymentService.Core.Entities;
using PaymentService.Core.Interfaces;
using PaymentService.Infrastructure.Data;

namespace PaymentService.Infrastructure.Repositories;

public class PaymentRepository : IPaymentRepository
{
    private readonly PaymentDbContext _context;
    private readonly ILogger<PaymentRepository> _logger;

    public PaymentRepository(PaymentDbContext context, ILogger<PaymentRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Payment?> GetByIdAsync(int id)
    {
        _logger.LogInformation("[GetByIdAsync] Fetching payment ID: {PaymentId}", id);
        return await _context.Payments.FindAsync(id);
    }

    public async Task<IEnumerable<Payment>> GetAllAsync()
    {
        _logger.LogInformation("[GetAllAsync] Fetching all payments");
        return await _context.Payments.ToListAsync();
    }

    public async Task<IEnumerable<Payment>> GetByStatusAsync(string status)
    {
        _logger.LogInformation("[GetByStatusAsync] Fetching payments with status: {Status}", status);
        return await _context.Payments
            .Where(p => p.Status == status)
            .ToListAsync();
    }

    public async Task AddAsync(Payment payment)
    {
        _logger.LogInformation("[AddAsync] Adding new payment for Booking: {BookingId}, Amount: {Amount}", payment.BookingId, payment.Amount);
        await _context.Payments.AddAsync(payment);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Payment payment)
    {
        _logger.LogInformation("[UpdateAsync] Updating payment ID: {PaymentId}", payment.Id);
        _context.Payments.Update(payment);
        await _context.SaveChangesAsync();
    }
}
