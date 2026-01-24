using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotificationService.Core.Dtos;
using NotificationService.Core.Interfaces;

namespace NotificationService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class NotificationsController : ControllerBase
{
    private readonly INotificationService _notificationService;
    private readonly ILogger<NotificationsController> _logger;

    public NotificationsController(INotificationService notificationService, ILogger<NotificationsController> logger)
    {
        _notificationService = notificationService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> CreateNotification([FromBody] CreateNotificationDto createDto)
    {
        _logger.LogInformation("POST /api/Notifications called with Type={Type}, Message={Message}, Receiver={Receiver}", createDto.Type, createDto.Message, createDto.Receiver);

        var result = await _notificationService.CreateNotificationAsync(createDto);

        if (!result.Success)
        {
            _logger.LogWarning("Failed to create notification: {ErrorMessage}", result.ErrorMessage);
            return BadRequest(result.ErrorMessage);
        }

        _logger.LogInformation("Notification created successfully: Id={Id}", result.Data?.Id);
        return CreatedAtAction(nameof(GetById), new { id = result.Data?.Id }, result.Data);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var notifications = await _notificationService.GetAllNotificationsAsync();
        return Ok(notifications);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var notification = await _notificationService.GetNotificationByIdAsync(id);
        if (notification == null) return NotFound();
        return Ok(notification);
    }
}
