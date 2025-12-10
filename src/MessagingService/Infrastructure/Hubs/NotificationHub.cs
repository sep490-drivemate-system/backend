using MessagingService.Application.Commons.DTOs.Notification;
using MessagingService.Application.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SharedLibrary.SharedKernel.Enum;
using SharedLibrary.SharedKernel.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace MessagingService.Infrastructure.Hubs
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class NotificationHub(INotificationUseCase notificationUseCase, IUserClaimsAccessor userClaimsAccessor) : Hub
    {
        private readonly INotificationUseCase _notificationUseCase = notificationUseCase;
        private readonly IUserClaimsAccessor _userClaimsAccessor = userClaimsAccessor;

        public override async Task OnConnectedAsync()
        {
            var userId = _userClaimsAccessor.GetUserId(Context.User);
            if (userId == null)
            {
                Context.Abort();
                return;
            }

            await Groups.AddToGroupAsync(Context.ConnectionId, BuildGroupName(userId));
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = _userClaimsAccessor.GetUserId(Context.User);

            if (userId != null)
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, BuildGroupName(userId));
            }

            await base.OnDisconnectedAsync(exception);
        }

        public async Task<IEnumerable<NotificationDTO>> GetUserNotificationsAsync(int pageNumber = 1, int pageSize = 10)
        {
            var userId = _userClaimsAccessor.GetUserId(Context.User);
            if (userId == null)
            {
                return Enumerable.Empty<NotificationDTO>();
            }
            var result = await _notificationUseCase.GetUserNotificationsAsync(userId, pageNumber, pageSize);
            return result;
        }

        public async Task<int> GetUnreadNotificationCountAsync()
        {
            var userId = _userClaimsAccessor.GetUserId(Context.User);
            if (userId == null)
            {
                return 0;
            }

            var count = await _notificationUseCase.GetUnreadNotificationCountAsync(userId);
            return count;
        }

        public async Task<bool> MarkNotificationAsReadAsync(Guid notificationId)
        {
            var userId = _userClaimsAccessor.GetUserId(Context.User);
            if (userId == null)
            {
                return false;
            }

            var result = await _notificationUseCase.MarkNotificationAsReadAsync(notificationId, userId);
           
            var unreadCount = await _notificationUseCase.GetUnreadNotificationCountAsync(userId);
            await Clients.Caller.SendAsync("UnreadCountUpdated", unreadCount);
            
            return result;
        }

        public async Task<bool> MarkAllNotificationsAsReadAsync()
        {
            var userId = _userClaimsAccessor.GetUserId(Context.User);
            if (userId == null)
            {
                return false;
            }

            var result = await _notificationUseCase.MarkAllNotificationsAsReadAsync(userId);
            
            await Clients.Caller.SendAsync("UnreadCountUpdated", 0);
            
            return result;
        }

        public async Task<NotificationDTO> CreateNotification(CreateNotificationDTO dto)
        {
            var notification = await _notificationUseCase.CreateNotificationAsync(dto);
           
            await Clients.Group(BuildGroupName(dto.UserId))
                .SendAsync("NewNotification", notification);            
            var unreadCount = await _notificationUseCase.GetUnreadNotificationCountAsync(dto.UserId);
            await Clients.Group(BuildGroupName(dto.UserId))
                .SendAsync("UnreadCountUpdated", unreadCount);
            
            return notification;
        }

        public static string BuildGroupName(Guid userId) => $"user_{userId}";


    }
}

