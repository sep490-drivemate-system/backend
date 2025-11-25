using MessagingService.Application.Commons.DTOs.Chat;
using SharedLibrary.SharedKernel.ServiceResult;

namespace MessagingService.Application.Interfaces
{
    public interface IChatUseCase
    {
        Task<Result<ConversationResponseDTO>> GetOrCreateConversationAsync(Guid userId1, Guid userId2);
        Task<Result<IEnumerable<ConversationResponseDTO>>> GetUserConversationsAsync(Guid userId);
        Task<Result<IEnumerable<MessageResponseDTO>>> GetConversationMessagesAsync(Guid conversationId, Guid userId, int pageNumber = 1, int pageSize = 50);
        Task<Result<MessageResponseDTO>> SendMessageAsync(SendMessageDTO dto, Guid senderId);
        Task<Result<bool>> MarkMessagesAsReadAsync(Guid conversationId, Guid userId);
        Task<Result<int>> GetUnreadMessageCountAsync(Guid conversationId, Guid userId);
    }
}

