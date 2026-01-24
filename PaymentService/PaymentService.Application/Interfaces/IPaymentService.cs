using PaymentService.Application.Common;
using PaymentService.Application.Dtos;

namespace PaymentService.Application.Interfaces
{
    public interface IPaymentService
    {
        Task<ServiceResult<PaymentDto>> AddPaymentAsync(AddPaymentDto addPaymentDto);
        Task<IEnumerable<PaymentDto>> GetPaymentsAsync(string? status);
    }
}

