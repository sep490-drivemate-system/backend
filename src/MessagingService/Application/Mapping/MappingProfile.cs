using AutoMapper;
using MessagingService.Application.DTOs.Notification;
using MessagingService.Domain.Entities;

namespace MessagingService.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Notification, NotificationResponseDTO>();
            CreateMap<CreateNotificationDTO, Notification>();
        }
    }
}

