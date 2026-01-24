using Microsoft.Extensions.Logging;
using RoomService.Core.Dtos;
using RoomService.Core.Entities;
using RoomService.Core.Interfaces;

namespace RoomService.Core.Services;

public class RoomsService
{
    private readonly IRoomRepository _roomRepository;
    private readonly ILogger<RoomsService> _logger;

    public RoomsService(IRoomRepository roomRepository, ILogger<RoomsService> logger)
    {
        _roomRepository = roomRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<RoomDto>> GetAllRoomsAsync()
    {
        _logger.LogInformation("[GetAllRoomsAsync] Fetching all rooms");
        var rooms = await _roomRepository.GetAllAsync();
        
        return rooms.Select(r => new RoomDto(r.Id, r.Name, r.Description, r.PricePerNight, r.IsAvailable));
    }

    public async Task<RoomDto?> GetRoomByIdAsync(int id)
    {
        _logger.LogInformation("[GetRoomByIdAsync] Fetching room ID: {RoomId}", id);
        var room = await _roomRepository.GetByIdAsync(id);
        
        return room == null ? null : new RoomDto(room.Id, room.Name, room.Description, room.PricePerNight, room.IsAvailable);
    }

    public async Task<RoomDto> CreateRoomAsync(CreateRoomDto createRoomDto)
    {
        _logger.LogInformation("[CreateRoomAsync] Creating new room: {RoomName}", createRoomDto.Name);
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
        _logger.LogInformation("[UpdateRoomAsync] Updating room ID: {RoomId}", id);
        var room = await _roomRepository.GetByIdAsync(id);
        
        if (room == null)
        {
            _logger.LogWarning("[UpdateRoomAsync] Room ID: {RoomId} not found", id);
            return null;
        }

        room.Name = updateRoomDto.Name;
        room.Description = updateRoomDto.Description;
        room.PricePerNight = updateRoomDto.PricePerNight;
        room.IsAvailable = updateRoomDto.IsAvailable;

        await _roomRepository.UpdateAsync(room);
        
        return new RoomDto(room.Id, room.Name, room.Description, room.PricePerNight, room.IsAvailable);
    }

    public async Task<bool> DeleteRoomAsync(int id)
    {
        _logger.LogInformation("[DeleteRoomAsync] Deleting room ID: {RoomId}", id);
        var room = await _roomRepository.GetByIdAsync(id);
        
        if (room == null)
        {
            _logger.LogWarning("[DeleteRoomAsync] Room ID: {RoomId} not found for deletion", id);
            return false;
        }

        await _roomRepository.DeleteAsync(id);
        
        return true;
    }
}
