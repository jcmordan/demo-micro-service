namespace RoomService.Core.Entities;

/**
 * Represents a room in the system.
 */
public class Room
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal PricePerNight { get; set; }
    public bool IsAvailable { get; set; } = true;
}
