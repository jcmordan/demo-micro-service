namespace NotificationService.Core.Dtos;

public record NotificationDto(int Id, string Message, string Type, string Receiver, DateTime CreatedAt);
public record CreateNotificationDto(string Message, string Type, string Receiver);
