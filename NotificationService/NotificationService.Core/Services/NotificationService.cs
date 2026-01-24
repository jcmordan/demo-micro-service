using NotificationService.Core.Dtos;
using NotificationService.Core.Entities;
using NotificationService.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace NotificationService.Core.Services;

public class NotificationService : INotificationService
{
    private readonly INotificationRepository _notificationRepository;
    private readonly ILogger<NotificationService> _logger;

    public NotificationService(INotificationRepository notificationRepository, ILogger<NotificationService> logger)
    {
        _notificationRepository = notificationRepository;
        _logger = logger;
    }

    public async Task<ServiceResult<NotificationDto>> CreateNotificationAsync(CreateNotificationDto createDto)
    {
        try
        {
            _logger.LogInformation("Creating notification: Type={Type}, Message={Message}, Receiver={Receiver}", createDto.Type, createDto.Message, createDto.Receiver);

            var notification = new Notification
            {
                Message = createDto.Message,
                Type = createDto.Type,
                Receiver = createDto.Receiver,
                CreatedAt = DateTime.UtcNow
            };

            var createdNotification = await _notificationRepository.AddAsync(notification);

            _logger.LogInformation("Notification created successfully: Id={Id}", createdNotification.Id);

            var notificationDto = new NotificationDto(
                createdNotification.Id,
                createdNotification.Message,
                createdNotification.Type,
                createdNotification.Receiver,
                createdNotification.CreatedAt
            );

            return ServiceResult<NotificationDto>.SuccessResult(notificationDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating notification: {ErrorMessage}", ex.Message);
            return ServiceResult<NotificationDto>.Failure($"Failed to create notification: {ex.Message}");
        }
    }

    public async Task<IEnumerable<NotificationDto>> GetAllNotificationsAsync()
    {
        _logger.LogInformation("Retrieving all notifications");
        var notifications = await _notificationRepository.GetAllAsync();
        return notifications.Select(n => new NotificationDto(n.Id, n.Message, n.Type, n.Receiver, n.CreatedAt));
    }

    public async Task<NotificationDto?> GetNotificationByIdAsync(int id)
    {
        _logger.LogInformation("Retrieving notification: Id={Id}", id);
        var notification = await _notificationRepository.GetByIdAsync(id);
        return notification == null ? null : new NotificationDto(notification.Id, notification.Message, notification.Type, notification.Receiver, notification.CreatedAt);
    }
}
