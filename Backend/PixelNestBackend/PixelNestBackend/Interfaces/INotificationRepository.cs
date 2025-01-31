using PixelNestBackend.Dto;
using PixelNestBackend.Dto.Projections;

namespace PixelNestBackend.Interfaces
{
    public interface INotificationRepository
    {
        ICollection<ResponseNotificationsDto> GetNotifications(int userID);
        int CountNotifications(int userID);

        bool MarkAsOpened(MarkAsOpenedDto markAsRead, int userID);
    }
}
