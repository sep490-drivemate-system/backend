using System;
using System.Collections.Generic;
using System.Linq;
using MessagingService.Application.DTOs.Notification;
using MessagingService.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace MessagingService.Infrastructure.Hubs
{
    [AllowAnonymous]
    public class NotificationHub : Hub
    {
        private readonly INotificationUseCase _notificationUseCase;

        public NotificationHub(INotificationUseCase notificationUseCase)
        {
            _notificationUseCase = notificationUseCase;
        }

        public override async Task OnConnectedAsync()
        {
            var userIdParam = Context.GetHttpContext()?.Request.Query["userId"].ToString();

            if (Guid.TryParse(userIdParam, out var userId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, BuildGroupName(userId));
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userIdParam = Context.GetHttpContext()?.Request.Query["userId"].ToString();

            if (Guid.TryParse(userIdParam, out var userId))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, BuildGroupName(userId));
            }

            await base.OnDisconnectedAsync(exception);
        }

        public async Task<IEnumerable<NotificationResponseDTO>> GetLatestNotifications(Guid userId, int take = 20)
        {
            var result = await _notificationUseCase.GetUserNotificationsAsync(userId, 1, take);
            return result.IsSuccess && result.Data != null
                ? result.Data
                : Enumerable.Empty<NotificationResponseDTO>();
        }

        public static string BuildGroupName(Guid userId) => $"user_{userId}";
    }
}

