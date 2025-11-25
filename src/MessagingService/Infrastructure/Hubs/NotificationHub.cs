using MessagingService.Application.Commons.DTOs.Notification;
using MessagingService.Application.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SharedLibrary.SharedKernel.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace MessagingService.Infrastructure.Hubs
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class NotificationHub(INotificationUseCase notificationUseCase) : Hub
    {
        private readonly INotificationUseCase _notificationUseCase = notificationUseCase;

        public override async Task OnConnectedAsync()
        {
            var userId = GetAuthenticatedUserId();
            var userRole = GetUserRole();
            if (userId == null)
            {
                Context.Abort();
                return;
            }

            await Groups.AddToGroupAsync(Context.ConnectionId, BuildGroupName(userId.Value));
            Console.WriteLine("lây id " + userId);
            Console.WriteLine("lấy role " + userRole);

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = GetAuthenticatedUserId();

            if (userId != null)
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, BuildGroupName(userId.Value));
            }

            await base.OnDisconnectedAsync(exception);
        }

        public async Task<IEnumerable<NotificationResponseDTO>> GetLatestNotifications(int take = 20)
        {
            var userId = GetAuthenticatedUserId();

            if (userId == null)
            {
                throw new HubException("Unauthorized access to notifications.");
            }

            var result = await _notificationUseCase.GetUserNotificationsAsync(userId.Value, 1, take);
            return result.IsSuccess && result.Data != null
                ? result.Data
                : Enumerable.Empty<NotificationResponseDTO>();
        }

        public static string BuildGroupName(Guid userId) => $"user_{userId}";

        private Guid? GetAuthenticatedUserId()
        {
            var idClaim = Context.User?.Claims.FirstOrDefault(c => c.Type == "id");
            return idClaim != null && Guid.TryParse(idClaim.Value, out var userId)
                ? userId
                : null;
        }
        private UserRole? GetUserRole()
        {
            var roleClaim = Context.User?.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.Role || c.Type == "role");

            if (roleClaim != null && Enum.TryParse<UserRole>(roleClaim.Value, out var role))
                return role;

            return null;
        }


    }
}

