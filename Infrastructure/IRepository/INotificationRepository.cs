using Core.Model;
using Core.ModelDto.Notification;

namespace Infrastructure.IRepository;

public interface INotificationRepository
{
    Task<PaginatedListModel<NotificationResponseDto>> GetNotifications(int pageNumber, NotificationFilterDto searchModel);
    Task<List<NotificationResponseDto>> GetDistinctNotifications(string userId);
    Task<NotificationResponseDto> GetNotificationById(int NotificationId);
    Task<int> InsertNotification(NotificationRequestDto insertRequestModel);
    Task<int> UpdateNotification(int NotificationId, NotificationRequestDto updateRequestModel);
    Task<int> DeleteNotification(int NotificationId, NotificationRequestDto deleteRequestModel);
}
