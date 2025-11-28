using AutoMapper;
using MessagingService.Application.Commons.DTOs.Chat;
using MessagingService.Application.Commons.DTOs.Chat;
using MessagingService.Application.Commons.DTOs.Notification;
using MessagingService.Domain.Entities;
using MessagingService.Domain.Enum;

namespace MessagingService.Application.Commons.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Notification, NotificationResponseDTO>();
            CreateMap<CreateNotificationDTO, Notification>();
            CreateMap<SendMessageDTO, Message>()
                .ForMember(dest => dest.ChatSessionId, opt => opt.MapFrom(src => src.ConversationId))
                .ForMember(dest => dest.SenderId, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.MapFrom(_ => MessageStatus.Sent))
                .ForMember(dest => dest.LastModifiedAt, opt => opt.Ignore());
            CreateMap<CreateChatSessionDTO, ChatSession>();
        }
    }
}

