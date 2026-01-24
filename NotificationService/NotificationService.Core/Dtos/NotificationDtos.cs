namespace NotificationService.Core.Dtos;

public record NotificationDto(int Id, string Message, string Type, DateTime CreatedAt);
public record CreateNotificationDto(string Message, string Type);
