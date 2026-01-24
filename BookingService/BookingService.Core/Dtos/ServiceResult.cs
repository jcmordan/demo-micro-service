namespace BookingService.Core.Dtos;

public record ServiceResult<T>(bool Success, T? Data = default, string? ErrorMessage = null)
{
    public static ServiceResult<T> SuccessResult(T data) => new(true, data);
    public static ServiceResult<T> Failure(string message) => new(false, default, message);
}
