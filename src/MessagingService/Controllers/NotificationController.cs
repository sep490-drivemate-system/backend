using MessagingService.Application.DTOs.Notification;
using MessagingService.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Jwt;
using SharedLibrary.SharedKernel.ServiceResult;

namespace MessagingService.Controllers
{
    [ApiController]
    [Route("api/notifications")]
    [Authorize]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationUseCase _notificationUseCase;
        private readonly IJwtService _jwtService;

        public NotificationController(INotificationUseCase notificationUseCase, IJwtService jwtService)
        {
            _notificationUseCase = notificationUseCase;
            _jwtService = jwtService;
        }

        [HttpGet]
        public async Task<IActionResult> GetNotifications([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 50)
        {
            var userId = await _jwtService.ExtractUserIdFromToken(Request.Headers["Authorization"].ToString());
            var result = await _notificationUseCase.GetUserNotificationsAsync(userId, pageNumber, pageSize);
            return result.ToActionResult();
        }

        [HttpGet("unread-count")]
        public async Task<IActionResult> GetUnreadCount()
        {
            var userId = await _jwtService.ExtractUserIdFromToken(Request.Headers["Authorization"].ToString());
            var result = await _notificationUseCase.GetUnreadNotificationCountAsync(userId);
            return result.ToActionResult();
        }

        [HttpPost("{notificationId}/read")]
        public async Task<IActionResult> MarkAsRead([FromRoute] Guid notificationId)
        {
            var userId = await _jwtService.ExtractUserIdFromToken(Request.Headers["Authorization"].ToString());
            var result = await _notificationUseCase.MarkNotificationAsReadAsync(notificationId, userId);
            return result.ToActionResult();
        }

        [HttpPost("read-all")]
        public async Task<IActionResult> MarkAllAsRead()
        {
            var userId = await _jwtService.ExtractUserIdFromToken(Request.Headers["Authorization"].ToString());
            var result = await _notificationUseCase.MarkAllNotificationsAsReadAsync(userId);
            return result.ToActionResult();
        }

        // Internal endpoint for other services to create notifications
        [HttpPost("create")]
        [AllowAnonymous] // You might want to add service-to-service authentication here
        public async Task<IActionResult> CreateNotification([FromBody] CreateNotificationDTO dto)
        {
            var result = await _notificationUseCase.CreateNotificationAsync(dto);
            return result.ToActionResult();
        }
    }
}

