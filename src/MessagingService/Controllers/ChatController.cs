using MessagingService.Application.DTOs.Chat;
using MessagingService.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Jwt;
using SharedLibrary.SharedKernel.ServiceResult;

namespace MessagingService.Controllers
{
    [ApiController]
    [Route("api/chat")]
    [Authorize]
    public class ChatController : ControllerBase
    {
        private readonly IChatUseCase _chatUseCase;
        private readonly IJwtService _jwtService;

        public ChatController(IChatUseCase chatUseCase, IJwtService jwtService)
        {
            _chatUseCase = chatUseCase;
            _jwtService = jwtService;
        }

        [HttpGet("conversations")]
        public async Task<IActionResult> GetConversations()
        {
            var userId = await _jwtService.ExtractUserIdFromToken(Request.Headers["Authorization"].ToString());
            var result = await _chatUseCase.GetUserConversationsAsync(userId);
            return result.ToActionResult();
        }

        [HttpPost("conversations")]
        public async Task<IActionResult> GetOrCreateConversation([FromBody] GetOrCreateConversationRequest request)
        {
            var userId = await _jwtService.ExtractUserIdFromToken(Request.Headers["Authorization"].ToString());
            var result = await _chatUseCase.GetOrCreateConversationAsync(userId, request.OtherUserId);
            return result.ToActionResult();
        }

        [HttpGet("conversations/{conversationId}/messages")]
        public async Task<IActionResult> GetMessages([FromRoute] Guid conversationId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 50)
        {
            var userId = await _jwtService.ExtractUserIdFromToken(Request.Headers["Authorization"].ToString());
            var result = await _chatUseCase.GetConversationMessagesAsync(conversationId, userId, pageNumber, pageSize);
            return result.ToActionResult();
        }

        [HttpPost("messages")]
        public async Task<IActionResult> SendMessage([FromBody] SendMessageDTO dto)
        {
            var userId = await _jwtService.ExtractUserIdFromToken(Request.Headers["Authorization"].ToString());
            var result = await _chatUseCase.SendMessageAsync(dto, userId);
            return result.ToActionResult();
        }

        [HttpPost("conversations/{conversationId}/read")]
        public async Task<IActionResult> MarkAsRead([FromRoute] Guid conversationId)
        {
            var userId = await _jwtService.ExtractUserIdFromToken(Request.Headers["Authorization"].ToString());
            var result = await _chatUseCase.MarkMessagesAsReadAsync(conversationId, userId);
            return result.ToActionResult();
        }

        [HttpGet("conversations/{conversationId}/unread-count")]
        public async Task<IActionResult> GetUnreadCount([FromRoute] Guid conversationId)
        {
            var userId = await _jwtService.ExtractUserIdFromToken(Request.Headers["Authorization"].ToString());
            var result = await _chatUseCase.GetUnreadMessageCountAsync(conversationId, userId);
            return result.ToActionResult();
        }
    }

    public class GetOrCreateConversationRequest
    {
        public Guid OtherUserId { get; set; }
    }
}

