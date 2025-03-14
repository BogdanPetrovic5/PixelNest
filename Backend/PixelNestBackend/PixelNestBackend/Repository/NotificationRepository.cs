using Microsoft.EntityFrameworkCore;
using PixelNestBackend.Data;
using PixelNestBackend.Dto;
using PixelNestBackend.Dto.Projections;
using PixelNestBackend.Interfaces;
using PixelNestBackend.Security;

namespace PixelNestBackend.Repository
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly DataContext _dataContext;
        private readonly SASTokenGenerator _tokenGenerator;
        public NotificationRepository(DataContext dataContext, SASTokenGenerator tokenGenerator)
        {
            _dataContext = dataContext;
            _tokenGenerator = tokenGenerator;
        }

        public int CountNotifications(string userGuid)
        {
            try
            {
                int notifications = _dataContext.Notifications.Where(u => (u.ReceiverGuid).ToString() == userGuid && u.IsNew == true).Count();
                return notifications;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }
        }

        public ICollection<ResponseNotificationsDto> GetNotifications(string userGuid)
        {
            try
            {
                ICollection<ResponseNotificationsDto>? responseNotifications = null;

                responseNotifications = _dataContext.Notifications
                    .Where(u => (u.ReceiverGuid).ToString() == userGuid)
                    .Include(u => u.ReceiverUser)
                    .Include(u => u.SenderUser)
                    .Include(u => u.Post)
                    .Select(u => new ResponseNotificationsDto
                    {
                        Date = u.DateTime,
                        Message = u.Message,
                        NotificationID = u.NotificaitonID,
                        Username = u.SenderUser.Username,
                        PostID = u.PostGuid,
                        ImagePath = u.Post.ImagePaths.Select(l => new ResponseImageDto
                        {
                            Path = l.Path,
                            PhotoDisplay = l.PhotoDisplay,
                            PathID = l.PathID

                        }).ToList()
                    }).ToList();
                responseNotifications = responseNotifications.OrderByDescending(a => a.Date).ToList();
                //foreach (var notification in responseNotifications)
                //{
                //    _tokenGenerator.appendSasToken(notification.ImagePath);
                //}
                return responseNotifications;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        public bool MarkAsOpened(MarkAsOpenedDto markAsRead, string userGuid)
        {
            try
            {
                var notificationIDs = markAsRead.NotificationID;


                var notificationsToUpdate = _dataContext.Notifications
                        .Where(nid => notificationIDs.Contains(nid.NotificaitonID) && (nid.ReceiverGuid).ToString() == userGuid)
                        .ToList();
                foreach (var notification in notificationsToUpdate)
                {
                    notification.IsNew = false; 
                }


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
