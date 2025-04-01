using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using PixelNestBackend.Data;
using PixelNestBackend.Dto;
using PixelNestBackend.Dto.Projections;
using PixelNestBackend.Interfaces;
using PixelNestBackend.Models;
using PixelNestBackend.Responses;
using System.Linq;
using System.Linq.Expressions;

namespace PixelNestBackend.Repository
{
    public class ChatRepository : IChatRepository
    {
        private readonly DataContext _dataContext;
        private IMemoryCache _memoryCache;
      
        private const string MessagesCache = "Messages_{0}";
        private readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(10);
        private readonly ILogger<IChatRepository> _logger;
        public ChatRepository(
            DataContext dataContext,
            IMemoryCache memoryCache,
            ILogger<IChatRepository> logger
            )
        {
            _dataContext = dataContext;
            _memoryCache = memoryCache;
            _logger = logger;
        }

        public int GetNumberOfNewMessages(string userGuid)
        {
            try
            {
                int number = _dataContext.SeenMessages.Where(u => (u.UserGuid).ToString() == userGuid)
                    .GroupBy(u => u.SenderID)
                    .Count();
                return number;
                    
            }catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return 0;
            }
        }

        public ICollection<ResponseChatsDto> GetUserChats(string userGuid)
        {

                ICollection<ResponseChatsDto> userChats = _dataContext.Messages
                   .Where(m => (m.SenderGuid).ToString() == userGuid || (m.ReceiverGuid).ToString() == userGuid)
                   .Include(u => u.Sender)
                   .Include(u => u.Receiver)
                   .ToList()
                  .GroupBy(m => m.ChatID)
                   .Select(group => new ResponseChatsDto
                   {
                       ChatID = group.Key,
                       UserID = group.First().SenderGuid.ToString() == userGuid ? (group.First().Receiver.ClientGuid).ToString() : (group.First().ReceiverGuid).ToString() == userGuid ? (group.First().Sender.ClientGuid).ToString() : "",
                       Messages = group


                           .OrderByDescending(m => m.DateSent)
                           .Take(1)
                           .Select(m => new ResponseMessagesDto
                           {
                               Sender = m.Sender.Username,
                               Receiver = m.Receiver.Username,
                               Message = m.MessageText,
                               DateSent = m.DateSent,
                               Source = (m.SenderGuid).ToString() == userGuid ? m.Receiver.Username : (m.ReceiverGuid).ToString() == userGuid ? m.Sender.Username : "",
                               UserID = (m.SenderGuid).ToString() == userGuid ? (m.Receiver.ClientGuid).ToString() : (m.ReceiverGuid).ToString() == userGuid ? (m.Sender.ClientGuid).ToString() : "",
                               MessageID = m.MessageID,
                               IsSeen = !_dataContext.SeenMessages
                               .Any(sm => (sm.UserGuid).ToString() == userGuid && sm.MessageID == m.MessageID)

                           })
                           .ToList()
                   })
                   .OrderByDescending(date => date.Messages.First().DateSent)
                   .ToList();
             
          

               
            
            return userChats;
        }

        public ICollection<ResponseMessagesDto> GetUserToUserMessages(string chatID,Guid userID)
        {
            try
            {
                var cacheKey = string.Format(MessagesCache, chatID, userID.ToString());
                var versionKey = $"{cacheKey}_Version";
                if (!_memoryCache.TryGetValue(versionKey, out DateTime cachedVersion))
                {
                    cachedVersion = DateTime.MinValue;
                }
                else cachedVersion = DateTime.MaxValue;
                var latestVersion = DateTime.UtcNow;
                if (!_memoryCache.TryGetValue(cacheKey, out ICollection<ResponseMessagesDto> cashedMessages) || cachedVersion < latestVersion)
                {
                    cashedMessages = _dataContext.Messages
                   .Where(u => u.ChatID.Equals(chatID))
                   .Select(m => new ResponseMessagesDto
                   {
                       Sender = m.Sender.Username,
                       Receiver = m.Receiver.Username,
                       DateSent = m.DateSent,
                       Message = m.MessageText,
                       MessageID = m.MessageID,
                       UserID = (m.Sender.ClientGuid).ToString(),
                       IsSeen = !_dataContext.SeenMessages
                           .Any(sm => sm.UserGuid == userID && sm.MessageID == m.MessageID)
                   }).ToList();
                    _memoryCache.Set(cacheKey, cashedMessages, new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = CacheDuration
                    });
                    _memoryCache.Set(versionKey, latestVersion, new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = CacheDuration
                    });
                }
                
                return cashedMessages;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;

            }
        }

        public bool MarkAsRead(MarkAsRead markAsrReadDto, string userGuid)
        {
            try
            {
                var messageIds = markAsrReadDto.MessageID;

                var messagesToDelete = _dataContext.SeenMessages
                                                    .Where(mid => messageIds.Contains(mid.MessageID) && (mid.UserGuid).ToString() == userGuid);

                
                _dataContext.SeenMessages.RemoveRange(messagesToDelete);
                return _dataContext.SaveChanges() > 0;
            }catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
        }
        
        public bool SaveMessage(Message message, bool isUserInRoom)
        {

            message.DateSent = DateTime.UtcNow;
            _dataContext.Messages.Add(message);

            
            try
            {
                _dataContext.SaveChanges(); 
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving message: {ex.Message}");
                return false; 
            }

            
            if (!isUserInRoom)
            {
                SeenMessages seenMessages = new SeenMessages
                {
                    MessageID = message.MessageID,
                    UserGuid = message.ReceiverGuid,
                    SenderGuid = message.SenderGuid
                    
                };
                try
                {
                    _dataContext.SeenMessages.Add(seenMessages);
                    
                    _dataContext.SaveChanges();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error saving seen message: {ex.Message}");
                }
            }
            var cacheKey = string.Format(MessagesCache, message.ChatID, message.SenderGuid);
            var cacheKey_2 = string.Format(MessagesCache, message.ChatID, message.ReceiverGuid);
            var cacheKey_3 = string.Format(MessagesCache, message.SenderGuid);
            var cacheKey_4 = string.Format(MessagesCache, message.ReceiverGuid);
           
            _memoryCache.Remove(cacheKey);
            _memoryCache.Remove(cacheKey_2);
            _memoryCache.Remove(cacheKey_3);
            _memoryCache.Remove(cacheKey_4);
            return true;

        }

        public ICollection<ResponseChatsDto> SearchChats(string parameter, string userGuid)
        {
           
            return _getChats(m => 
            (m.SenderGuid.ToString() == userGuid || m.ReceiverGuid.ToString() == userGuid) 
            && (m.Sender.Username.Contains(parameter) || m.Receiver.Username.Contains(parameter)), 
            userGuid);
        }
        private ICollection<ResponseChatsDto> _getChats(Expression<Func<Message, bool>> filter, string userGuid)
        {

            ICollection<ResponseChatsDto> userChats = _dataContext.Messages
               .Where(filter)
               .Where(m => (m.SenderGuid).ToString() == userGuid || (m.ReceiverGuid).ToString() == userGuid)
               .Include(u => u.Sender)
               .Include(u => u.Receiver)
               .ToList()
              .GroupBy(m => m.ChatID)
               .Select(group => new ResponseChatsDto
               {
                   ChatID = group.Key,
                   UserID = group.First().SenderGuid.ToString() == userGuid ? (group.First().Receiver.ClientGuid).ToString() : (group.First().ReceiverGuid).ToString() == userGuid ? (group.First().Sender.ClientGuid).ToString() : "",
                   Messages = group


                       .OrderByDescending(m => m.DateSent)
                       .Take(1)
                       .Select(m => new ResponseMessagesDto
                       {
                           Sender = m.Sender.Username,
                           Receiver = m.Receiver.Username,
                           Message = m.MessageText,
                           DateSent = m.DateSent,
                           Source = (m.SenderGuid).ToString() == userGuid ? m.Receiver.Username : (m.ReceiverGuid).ToString() == userGuid ? m.Sender.Username : "",
                           UserID = (m.SenderGuid).ToString() == userGuid ? (m.Receiver.ClientGuid).ToString() : (m.ReceiverGuid).ToString() == userGuid ? (m.Sender.ClientGuid).ToString() : "",
                           MessageID = m.MessageID,
                           IsSeen = !_dataContext.SeenMessages
                           .Any(sm => (sm.UserGuid).ToString() == userGuid && sm.MessageID == m.MessageID)

                       })
                       .ToList()
               })
               .OrderByDescending(date => date.Messages.First().DateSent)
               .ToList();





            return userChats;
        }
    }
}
