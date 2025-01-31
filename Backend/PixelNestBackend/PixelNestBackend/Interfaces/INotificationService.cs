using PixelNestBackend.Dto;
using PixelNestBackend.Dto.Projections;

namespace PixelNestBackend.Interfaces
{
    public interface INotificationService
    {
        ICollection<ResponseNotificationsDto> GetNotifications(string email);
        int CountNotifications(string email);
        bool MarkAsRead(MarkAsOpenedDto markAsrReadDto, string email);
    }
}
