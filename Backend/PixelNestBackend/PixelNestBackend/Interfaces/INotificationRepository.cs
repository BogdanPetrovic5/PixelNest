using PixelNestBackend.Dto;
using PixelNestBackend.Dto.Projections;

namespace PixelNestBackend.Interfaces
{
    public interface INotificationRepository
    {
        ICollection<ResponseNotificationsDto> GetNotifications(string userGuid);
        int CountNotifications(string userGuid);

        bool MarkAsOpened(MarkAsOpenedDto markAsRead, string userGuid);
    }
}
