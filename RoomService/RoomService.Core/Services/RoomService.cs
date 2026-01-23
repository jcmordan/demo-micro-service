using RoomService.Core.Dtos;
using RoomService.Core.Entities;
using RoomService.Core.Interfaces;

namespace RoomService.Core.Services;

public class RoomService : IRoomService
{
    private readonly IRoomRepository _roomRepository;

    public RoomService(IRoomRepository roomRepository)
    {
        _roomRepository = roomRepository;
    }

    public async Task<IEnumerable<RoomDto>> GetAllRoomsAsync()
    {
        var rooms = await _roomRepository.GetAllAsync();
        return rooms.Select(r => new RoomDto(r.Id, r.Name, r.Description, r.PricePerNight, r.IsAvailable));
    }

    public async Task<RoomDto?> GetRoomByIdAsync(int id)
    {
        var room = await _roomRepository.GetByIdAsync(id);
        return room == null ? null : new RoomDto(room.Id, room.Name, room.Description, room.PricePerNight, room.IsAvailable);
    }

    public async Task<RoomDto> CreateRoomAsync(CreateRoomDto createRoomDto)
    {
        var room = new Room
        {
            Name = createRoomDto.Name,
            Description = createRoomDto.Description,
            PricePerNight = createRoomDto.PricePerNight,
            IsAvailable = true
        };
        await _roomRepository.AddAsync(room);
        return new RoomDto(room.Id, room.Name, room.Description, room.PricePerNight, room.IsAvailable);
    }

    public async Task<RoomDto?> UpdateRoomAsync(int id, UpdateRoomDto updateRoomDto)
    {
        var room = await _roomRepository.GetByIdAsync(id);
        if (room == null) return null;

        room.Name = updateRoomDto.Name;
        room.Description = updateRoomDto.Description;
        room.PricePerNight = updateRoomDto.PricePerNight;
        room.IsAvailable = updateRoomDto.IsAvailable;

        await _roomRepository.UpdateAsync(room);
        return new RoomDto(room.Id, room.Name, room.Description, room.PricePerNight, room.IsAvailable);
    }

    public async Task<bool> DeleteRoomAsync(int id)
    {
        var room = await _roomRepository.GetByIdAsync(id);
        if (room == null) return false;

        await _roomRepository.DeleteAsync(id);
        return true;
    }
}
