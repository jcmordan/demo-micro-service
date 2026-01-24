using PaymentService.Application.Interfaces;
using PaymentService.Domain.Entities;
using System.Collections.Concurrent;

namespace PaymentService.Infrastructure.Repositories
{
    public class InMemoryPaymentRepository : IPaymentRepository
    {
        private static readonly ConcurrentDictionary<int, Payment> _payments = new();
        private static int _nextId = 1;

        public Task<Payment?> GetByIdAsync(int id)
        {
            _payments.TryGetValue(id, out var payment);
            return Task.FromResult(payment);
        }

        public Task<IEnumerable<Payment>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<Payment>>(_payments.Values);
        }

        public Task<IEnumerable<Payment>> GetByStatusAsync(string status)
        {
            var filtered = _payments.Values
                .Where(p => string.Equals(p.Status, status, StringComparison.OrdinalIgnoreCase));

            return Task.FromResult<IEnumerable<Payment>>(filtered);
        }

        public Task AddAsync(Payment payment)
        {
            payment.Id = Interlocked.Increment(ref _nextId);
            _payments[payment.Id] = payment;
            return Task.CompletedTask;
        }

        public Task SaveChangesAsync() => Task.CompletedTask;
    }
}