namespace PaymentService.Application.Common
{
    public class ServiceResult<T>
    {
        public bool Success { get; private set; }
        public T? Data { get; private set; }
        public string? ErrorMessage { get; private set; }

        public static ServiceResult<T> SuccessResult(T data) => new() { Success = true, Data = data };
        public static ServiceResult<T> Failure(string errorMessage) => new() { Success = false, ErrorMessage = errorMessage };
    }
}

