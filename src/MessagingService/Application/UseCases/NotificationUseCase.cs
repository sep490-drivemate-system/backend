using AutoMapper;
using MessagingService.Application.Commons.DTOs.Notification;
using MessagingService.Application.Interfaces;
using MessagingService.Domain.Entities;
using MessagingService.Domain.Enum;
using MessagingService.Infrastructure.UoW;
using SharedLibrary.SharedKernel.ServiceResult;

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

        public async Task<Result<NotificationResponseDTO>> CreateNotificationAsync(CreateNotificationDTO dto)
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
            return Result<NotificationResponseDTO>.Success(responseDTO);
        }

        public async Task<Result<IEnumerable<NotificationResponseDTO>>> GetUserNotificationsAsync(
            Guid userId, int pageNumber = 1, int pageSize = 50)
        {
            var notifications = await _unitOfWork.NotificationRepository
                .GetUserNotificationsAsync(userId, pageNumber, pageSize);

            var notificationDTOs = _mapper.Map<IEnumerable<NotificationResponseDTO>>(notifications);
            return Result<IEnumerable<NotificationResponseDTO>>.Success(notificationDTOs);
        }

        public async Task<Result<int>> GetUnreadNotificationCountAsync(Guid userId)
        {
            var count = await _unitOfWork.NotificationRepository
                .GetUnreadNotificationCountAsync(userId);

            return Result<int>.Success(count);
        }

        public async Task<Result<bool>> MarkNotificationAsReadAsync(Guid notificationId, Guid userId)
        {
            var notification = await _unitOfWork.NotificationRepository.GetByIdAsync(notificationId);

            if (notification == null || notification.IsDeleted)
            {
                return Result<bool>.Failure(
                    ServiceError.NotFoundError("Notification not found"));
            }

            if (notification.UserId != userId)
            {
                return Result<bool>.Failure(
                    ServiceError.ForbiddenError("You can only mark your own notifications as read"));
            }

            await _unitOfWork.NotificationRepository.MarkNotificationAsReadAsync(notificationId);
            await _unitOfWork.CommitChangesAsync();

            return Result<bool>.Success(true);
        }

        public async Task<Result<bool>> MarkAllNotificationsAsReadAsync(Guid userId)
        {
            await _unitOfWork.NotificationRepository.MarkAllNotificationsAsReadAsync(userId);
            await _unitOfWork.CommitChangesAsync();

            return Result<bool>.Success(true);
        }
    }
}

