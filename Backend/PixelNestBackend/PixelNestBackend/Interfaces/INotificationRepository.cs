using PixelNestBackend.Dto;
using PixelNestBackend.Dto.Projections;

namespace PixelNestBackend.Interfaces
{
    public interface INotificationRepository
    {
        ICollection<ResponseNotificationsDto> GetNotifications(Guid userID);
        int CountNotifications(Guid userID);

        bool MarkAsOpened(MarkAsOpenedDto markAsRead, Guid userID);
    }
}
