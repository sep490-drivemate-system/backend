using AutoMapper;
using MessagingService.Application.Commons.DTOs.Notification;
using MessagingService.Domain.Entities;

namespace MessagingService.Application.Commons.Mapping
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

