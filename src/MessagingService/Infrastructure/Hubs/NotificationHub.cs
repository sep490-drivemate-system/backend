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

        public async Task<IEnumerable<NotificationResponseDTO>> GetLatestNotifications(int take = 20)
        {
            var userId = _userClaimsAccessor.GetUserId(Context.User);

            var result = await _notificationUseCase.GetUserNotificationsAsync(userId, 1, take);
            return result ;
        }

        public static string BuildGroupName(Guid userId) => $"user_{userId}";


    }
}

