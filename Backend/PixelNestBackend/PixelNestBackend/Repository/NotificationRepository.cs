using Microsoft.EntityFrameworkCore;
using PixelNestBackend.Data;
using PixelNestBackend.Dto;
using PixelNestBackend.Dto.Projections;
using PixelNestBackend.Interfaces;

namespace PixelNestBackend.Repository
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly DataContext _dataContext;
        public NotificationRepository(DataContext dataContext) { 
            _dataContext = dataContext;
        }

        public int CountNotifications(int userID)
        {
            try
            {
                int notifications = _dataContext.Notifications.Where(u => u.ReceiverID == userID).Count();
                return notifications;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }
        }

        public ICollection<ResponseNotificationsDto> GetNotifications(int userID)
        {
            try
            {
                ICollection<ResponseNotificationsDto>? responseNotifications = null;

                responseNotifications = _dataContext.Notifications
                    .Where(u => u.ReceiverID == userID)
                    .Include(u => u.ReceiverUser)
                    .Include(u => u.SenderUser)
                    .Select(u => new ResponseNotificationsDto
                    {
                        Date = u.DateTime,
                        Message = u.Message,
                        NotificationID = u.NotificaitonID,
                        Username = u.SenderUser.Username,
                        PostID = u.PostID
                    }).ToList();

                return responseNotifications;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        public bool MarkAsOpened(MarkAsOpenedDto markAsRead, int userID)
        {
            try
            {
                var notificationIDs = markAsRead.NotificationID;

            
                var notificationsToDelete = _dataContext.Notifications
                                                    .Where(nid => notificationIDs.Contains(nid.NotificaitonID) && nid.ReceiverID == userID);


                _dataContext.Notifications.RemoveRange(notificationsToDelete);
                return _dataContext.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
