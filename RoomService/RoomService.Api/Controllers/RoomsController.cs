using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoomService.Core.Dtos;
using RoomService.Core.Services;
using Microsoft.Extensions.Logging;

namespace RoomService.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class RoomsController : ControllerBase
{
    private readonly RoomsService _roomService;
    private readonly ILogger<RoomsController> _logger;

    public RoomsController(RoomsService roomService, ILogger<RoomsController> logger)
    {
        _roomService = roomService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RoomDto>>> GetAll()
    {
        _logger.LogInformation("[GetAll] Received request to fetch all rooms");
        var rooms = await _roomService.GetAllRoomsAsync();
        
        return Ok(rooms);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RoomDto>> GetById(int id)
    {
        _logger.LogInformation("[GetById] Fetching room ID: {RoomId}", id);
        var room = await _roomService.GetRoomByIdAsync(id);
        
        if (room == null)
        {
            _logger.LogWarning("[GetById] Room ID: {RoomId} not found", id);
            return NotFound();
        }
        
        return Ok(room);
    }

    [HttpPost]
    public async Task<ActionResult<RoomDto>> Create(CreateRoomDto createRoomDto)
    {
        _logger.LogInformation("[Create] Request to create room: {RoomName}", createRoomDto.Name);
        var room = await _roomService.CreateRoomAsync(createRoomDto);
        
        return CreatedAtAction(nameof(GetById), new { id = room.Id }, room);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<RoomDto>> Update(int id, UpdateRoomDto updateRoomDto)
    {
        _logger.LogInformation("[Update] Request to update room ID: {RoomId}", id);
        var room = await _roomService.UpdateRoomAsync(id, updateRoomDto);
        
        if (room == null)
        {
            _logger.LogWarning("[Update] Room ID: {RoomId} not found", id);
            return NotFound();
        }
        
        return Ok(room);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        _logger.LogInformation("[Delete] Request to delete room ID: {RoomId}", id);
        var deleted = await _roomService.DeleteRoomAsync(id);
        
        if (!deleted)
        {
            _logger.LogWarning("[Delete] Room ID: {RoomId} not found for deletion", id);
            return NotFound();
        }
        
        return NoContent();
    }
}
