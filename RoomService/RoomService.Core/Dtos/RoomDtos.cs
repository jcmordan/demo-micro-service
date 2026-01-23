namespace RoomService.Core.Dtos;

public record RoomDto(int Id, string Name, string Description, decimal PricePerNight, bool IsAvailable);
public record CreateRoomDto(string Name, string Description, decimal PricePerNight);
public record UpdateRoomDto(string Name, string Description, decimal PricePerNight, bool IsAvailable);
