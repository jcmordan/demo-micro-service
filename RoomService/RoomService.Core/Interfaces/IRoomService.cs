using RoomService.Core.Dtos;

namespace RoomService.Core.Interfaces;

public interface IRoomService
{
    Task<IEnumerable<RoomDto>> GetAllRoomsAsync();
    Task<RoomDto?> GetRoomByIdAsync(int id);
    Task<RoomDto> CreateRoomAsync(CreateRoomDto createRoomDto);
    Task<RoomDto?> UpdateRoomAsync(int id, UpdateRoomDto updateRoomDto);
    Task<bool> DeleteRoomAsync(int id);
}
