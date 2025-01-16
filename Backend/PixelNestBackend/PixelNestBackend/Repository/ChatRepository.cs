using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
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

        public ChatRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public int GetNumberOfNewMessages(int userID)
        {
            try
            {

                int number = _dataContext.SeenMessages.Where(u => u.UserID == userID)
                    .GroupBy(u => u.SenderID)
                    .Count();
                return number;
                    
            }catch(Exception ex)
            {
                return 0;
            }
        }

        public ICollection<ResponseChatsDto> GetUserChats(int userID)
        {
            Console.WriteLine(userID);
            var userChats = _dataContext.Messages
                .Where(m => m.SenderID == userID || m.ReceiverID == userID)
                .Include(u => u.Sender)
                .Include(u => u.Receiver)
                .ToList()
                .GroupBy(m => m.SenderID < m.ReceiverID
                    ? $"{m.SenderID}-{m.ReceiverID}"
                    : $"{m.ReceiverID}-{m.SenderID}")
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
                            Source = m.SenderID == userID ? m.Receiver.Username : m.ReceiverID == userID ? m.Sender.Username : "",
                            MessageID = m.MessageID,
                            IsSeen = !_dataContext.SeenMessages
                            .Any(sm => sm.UserID == userID && sm.MessageID == m.MessageID)

                        })
                        .ToList()
                })
                .OrderByDescending(date => date.Messages.First().DateSent)
                .ToList();
            Console.WriteLine(userChats.Count());
            return userChats;
        }

        public ICollection<ResponseMessagesDto> GetUserToUserMessages(int userID, int targetID)
        {
            try
            {
                ICollection<ResponseMessagesDto> messages = _dataContext.Messages
                    .Where(u => (u.SenderID == userID || u.ReceiverID == userID) && (u.SenderID == targetID || u.ReceiverID == targetID))
                    .Select(m => new ResponseMessagesDto
                    {
                        Sender = m.Sender.Username,
                        Receiver = m.Receiver.Username,
                        DateSent = m.DateSent,
                        Message = m.MessageText,
                        MessageID = m.MessageID,
                         IsSeen = !_dataContext.SeenMessages
                            .Any(sm => sm.UserID == userID && sm.MessageID == m.MessageID)
                    }).ToList();
                return messages;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;

            }
        }

        public bool MarkAsRead(MarkAsRead markAsrReadDto, int userID)
        {
            try
            {
                var messageIds = markAsrReadDto.MessageID;

                foreach(var uid in markAsrReadDto.MessageID)
                {
                    Console.WriteLine("User" + uid);
                }
                var messagesToDelete = _dataContext.SeenMessages
                                                    .Where(mid => messageIds.Contains(mid.MessageID) && mid.UserID == userID);

                
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
                    UserID = message.ReceiverID,
                    SenderID = message.SenderID
                    
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

            return true;

        }
    }
}
