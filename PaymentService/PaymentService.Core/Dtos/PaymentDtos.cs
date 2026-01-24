namespace PaymentService.Core.Dtos;

public record PaymentDto(int Id, int BookingId, decimal Amount, DateTime PaymentDate, string Status);
public record AddPaymentDto(int BookingId, decimal Amount);
