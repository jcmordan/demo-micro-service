using PaymentService.Application.Interfaces;

namespace PaymentService.Infrastructure.Validators
{
    public class BookingValidator : IBookingValidator
    {
        public Task<bool> ExistsAsync(int bookingId)
        {
            return Task.FromResult(true);
        }
    }
}