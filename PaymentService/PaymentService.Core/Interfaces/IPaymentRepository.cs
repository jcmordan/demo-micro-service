using PaymentService.Core.Entities;

namespace PaymentService.Core.Interfaces;

public interface IPaymentRepository
{
    Task<Payment?> GetByIdAsync(int id);
    Task<IEnumerable<Payment>> GetAllAsync();
    Task<IEnumerable<Payment>> GetByStatusAsync(string status);
    Task AddAsync(Payment payment);
    Task UpdateAsync(Payment payment);
}
