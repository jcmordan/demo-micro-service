namespace PaymentService.Application.Interfaces
{
    public interface IBookingValidator
    {
        Task<bool> ExistsAsync(int bookingId);
    }
}

