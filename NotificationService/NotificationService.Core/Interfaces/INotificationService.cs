using NotificationService.Core.Dtos;

namespace NotificationService.Core.Interfaces;

public interface INotificationService
{
    Task<ServiceResult<NotificationDto>> CreateNotificationAsync(CreateNotificationDto createDto);
    Task<IEnumerable<NotificationDto>> GetAllNotificationsAsync();
    Task<NotificationDto?> GetNotificationByIdAsync(int id);
}
