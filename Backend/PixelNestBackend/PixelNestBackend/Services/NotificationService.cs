using PixelNestBackend.Dto;
using PixelNestBackend.Dto.Projections;
using PixelNestBackend.Interfaces;
using PixelNestBackend.Utility;

namespace PixelNestBackend.Services
{
    public class NotificationService : INotificationService
    {
        private readonly UserUtility _userUtility;
        private readonly INotificationRepository _notificationRepository;
       
        public NotificationService(
            UserUtility userUtility,
            INotificationRepository notificationRepository
                
        ) { 
            _notificationRepository = notificationRepository;
            _userUtility = userUtility;
        }

        public int CountNotifications(string userGuid)
        {

            return _notificationRepository.CountNotifications(userGuid);
        }

        public ICollection<ResponseNotificationsDto> GetNotifications(string userGuid)
        {
        
            return _notificationRepository.GetNotifications(userGuid);
        }

        public bool MarkAsOpened(MarkAsOpenedDto markAsOpenedDto, string userGuid)
        {
       
            return _notificationRepository.MarkAsOpened(markAsOpenedDto, userGuid);
        }
    }
}
