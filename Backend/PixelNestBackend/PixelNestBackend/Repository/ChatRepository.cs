﻿using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using PixelNestBackend.Data;
using PixelNestBackend.Dto;
using PixelNestBackend.Dto.Projections;
using PixelNestBackend.Interfaces;
using PixelNestBackend.Models;
using PixelNestBackend.Responses;
using System.Linq;

namespace PixelNestBackend.Repository
{
    public class ChatRepository : IChatRepository
    {
        private readonly DataContext _dataContext;
        private IMemoryCache _memoryCache;
      
        private const string MessagesCache = "Messages_{0}";
        private readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(10);
        public ChatRepository(
            DataContext dataContext,
            IMemoryCache memoryCache                    
            )
        {
            _dataContext = dataContext;
            _memoryCache = memoryCache;
        }

        public int GetNumberOfNewMessages(Guid userID)
        {
            try
            {

                int number = _dataContext.SeenMessages.Where(u => u.UserGuid == userID)
                    .GroupBy(u => u.SenderID)
                    .Count();
                return number;
                    
            }catch(Exception ex)
            {
                return 0;
            }
        }

        public ICollection<ResponseChatsDto> GetUserChats(Guid userID)
        {
            var cacheKey = string.Format(MessagesCache, userID);
            var versionKey = $"{cacheKey}_Version";
            if (!_memoryCache.TryGetValue(versionKey, out DateTime cachedVersion))
            {
                cachedVersion = DateTime.MinValue;
            }
            else cachedVersion = DateTime.MaxValue;
            var latestVersion = DateTime.UtcNow;
            Console.WriteLine("\n\nUserID: " + userID + "\n\n");
            if (!_memoryCache.TryGetValue(cacheKey, out ICollection<ResponseChatsDto> cashedChats) || cachedVersion < latestVersion) {
                Console.WriteLine("Entered query");
                var userChats = _dataContext.Messages
                   .Where(m => m.SenderGuid == userID || m.ReceiverGuid == userID)
                   .Include(u => u.Sender)
                   .Include(u => u.Receiver)
                   .ToList()
                  .GroupBy(m => m.SenderGuid.CompareTo(m.ReceiverGuid) < 0
                         ? $"{m.SenderGuid}-{m.ReceiverGuid}"
                         : $"{m.ReceiverGuid}-{m.SenderGuid}")
                   .Select(group => new ResponseChatsDto
                   {
                       ChatID = group.Key,
                       Messages = group


                           .OrderByDescending(m => m.DateSent)
                           .Take(1)
                           .Select(m => new ResponseMessagesDto
                           {
                               Sender = m.Sender.Username,
                               Receiver = m.Receiver.Username,
                               Message = m.MessageText,
                               DateSent = m.DateSent,
                              Source = m.SenderGuid == userID ? m.Receiver.Username : m.ReceiverGuid == userID ? m.Sender.Username : "",

                               MessageID = m.MessageID,
                               IsSeen = !_dataContext.SeenMessages
                               .Any(sm => sm.UserGuid == userID && sm.MessageID == m.MessageID)

                           })
                           .ToList()
                   })
                   .OrderByDescending(date => date.Messages.First().DateSent)
                   .ToList();
                _memoryCache.Set(cacheKey, userChats, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = CacheDuration
                });
                _memoryCache.Set(versionKey, latestVersion, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = CacheDuration
                });
                cashedChats = userChats;
            }

               
            
            return cashedChats;
        }

        public ICollection<ResponseMessagesDto> GetUserToUserMessages(Guid userID, Guid targetID)
        {
            try
            {
                var cacheKey = string.Format(MessagesCache, userID, targetID);
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
                   .Where(u => (u.SenderGuid == userID || u.ReceiverGuid == userID) && (u.SenderGuid == targetID || u.ReceiverGuid == targetID))
                   .Select(m => new ResponseMessagesDto
                   {
                       Sender = m.Sender.Username,
                       Receiver = m.Receiver.Username,
                       DateSent = m.DateSent,
                       Message = m.MessageText,
                       MessageID = m.MessageID,
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

        public bool MarkAsRead(MarkAsRead markAsrReadDto, Guid userID)
        {
            try
            {
                var messageIds = markAsrReadDto.MessageID;

                foreach(var uid in markAsrReadDto.MessageID)
                {
                    Console.WriteLine("User" + uid);
                }
                var messagesToDelete = _dataContext.SeenMessages
                                                    .Where(mid => messageIds.Contains(mid.MessageID) && mid.UserGuid == userID);

                
                _dataContext.SeenMessages.RemoveRange(messagesToDelete);
                return _dataContext.SaveChanges() > 0;
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
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
            var cacheKey = string.Format(MessagesCache, message.SenderGuid, message.ReceiverGuid);
            var cacheKey_2 = string.Format(MessagesCache, message.ReceiverGuid, message.SenderGuid);
            var versionKey = $"{cacheKey}_Version";
            _memoryCache.Remove(cacheKey);
            _memoryCache.Remove(cacheKey_2);
            return true;

        }
    }
}
