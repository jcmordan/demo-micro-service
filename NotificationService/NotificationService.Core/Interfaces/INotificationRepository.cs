using NotificationService.Core.Entities;

namespace NotificationService.Core.Interfaces;

public interface INotificationRepository
{
    Task<Notification?> GetByIdAsync(int id);
    Task<IEnumerable<Notification>> GetAllAsync();
    Task<Notification> AddAsync(Notification notification);
}
