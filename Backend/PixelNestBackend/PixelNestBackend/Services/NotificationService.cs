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

        public int CountNotifications(string email)
        {
            string username = _userUtility.GetUserName(email);
            int userID = _userUtility.GetUserID(username);
            return _notificationRepository.CountNotifications(userID);
        }

        public ICollection<ResponseNotificationsDto> GetNotifications(string email)
        {
            string username = _userUtility.GetUserName(email);
            int userID = _userUtility.GetUserID(username);
            return _notificationRepository.GetNotifications(userID);
        }

        public bool MarkAsRead(MarkAsOpenedDto markAsrReadDto, string email)
        {
            string username = _userUtility.GetUserName(email);
            int userID = _userUtility.GetUserID(username);
            return _notificationRepository.MarkAsOpened(markAsrReadDto, userID);
        }
    }
}
