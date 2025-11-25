using System;
using MessagingService.Application.DTOs.Notification;
using MessagingService.Application.Interfaces;
using MessagingService.Infrastructure.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SharedLibrary.Jwt;
using SharedLibrary.SharedKernel.ServiceResult;

namespace MessagingService.Controllers
{
    [ApiController]
    [Route("api/notifications")]
    public class NotificationController : ControllerBase
    {
    }
}

