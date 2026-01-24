using PaymentService.Application.Common;
using PaymentService.Application.Dtos;
using PaymentService.Application.Interfaces;
using PaymentService.Domain.Entities;

namespace PaymentService.Application.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IBookingValidator _bookingValidator;

        public PaymentService(IPaymentRepository paymentRepository, IBookingValidator bookingValidator)
        {
            _paymentRepository = paymentRepository;
            _bookingValidator = bookingValidator;
        }

        public async Task<ServiceResult<PaymentDto>> AddPaymentAsync(AddPaymentDto addPaymentDto)
        {
            var bookingExists = await _bookingValidator.ExistsAsync(addPaymentDto.BookingId);
            if (!bookingExists) return ServiceResult<PaymentDto>.Failure("Booking not found.");

            var payment = new Payment
            {
                BookingId = addPaymentDto.BookingId,
                Amount = addPaymentDto.Amount,
                PaymentDate = DateTime.UtcNow,
                Status = "Completed"
            };

            await _paymentRepository.AddAsync(payment);
            await _paymentRepository.SaveChangesAsync();

            return ServiceResult<PaymentDto>.SuccessResult(new PaymentDto(
                payment.Id,
                payment.BookingId,
                payment.Amount,
                payment.PaymentDate,
                payment.Status));
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

}
