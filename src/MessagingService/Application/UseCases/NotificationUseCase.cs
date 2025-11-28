using AutoMapper;
using MessagingService.Application.Commons.DTOs.Notification;
using MessagingService.Application.Interfaces;
using MessagingService.Domain.Entities;
using MessagingService.Domain.Enum;
using MessagingService.Infrastructure.UoW;

namespace MessagingService.Application.UseCases
{
    public class NotificationUseCase : INotificationUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public NotificationUseCase(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<NotificationResponseDTO> CreateNotificationAsync(CreateNotificationDTO dto)
        {
            var notification = new Notification
            {
                UserId = dto.UserId,
                Title = dto.Title,
                Content = dto.Content,
                Type = dto.Type,
                Status = NotificationStatus.Unread,
                ActionUrl = dto.ActionUrl,
                LastModifiedAt = DateTime.UtcNow
            };

            await _unitOfWork.NotificationRepository.AddAsync(notification);
            await _unitOfWork.CommitChangesAsync();

            var responseDTO = _mapper.Map<NotificationResponseDTO>(notification);
            return responseDTO;
        }

        public async Task<IEnumerable<NotificationResponseDTO>> GetUserNotificationsAsync(
            Guid userId, int pageNumber = 1, int pageSize = 50)
        {
            var notifications = await _unitOfWork.NotificationRepository
                .GetUserNotificationsAsync(userId, pageNumber, pageSize);

            var notificationDTOs = _mapper.Map<IEnumerable<NotificationResponseDTO>>(notifications);
            return notificationDTOs;
        }

        public async Task<int> GetUnreadNotificationCountAsync(Guid userId)
        {
            var count = await _unitOfWork.NotificationRepository
                .GetUnreadNotificationCountAsync(userId);

            return count;
        }

        public async Task<bool> MarkNotificationAsReadAsync(Guid notificationId, Guid userId)
        {
            var notification = await _unitOfWork.NotificationRepository.GetByIdAsync(notificationId);

            await _unitOfWork.NotificationRepository.MarkNotificationAsReadAsync(notificationId);
            await _unitOfWork.CommitChangesAsync();

            return true;
        }

        public async Task<bool> MarkAllNotificationsAsReadAsync(Guid userId)
        {
            await _unitOfWork.NotificationRepository.MarkAllNotificationsAsReadAsync(userId);
            await _unitOfWork.CommitChangesAsync();

            return true;
        }
    }
}

