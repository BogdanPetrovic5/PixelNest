using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PixelNestBackend.Data;
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
                            Source = m.SenderID == userID ? m.Receiver.Username : m.ReceiverID == userID ? m.Sender.Username : ""
                        })
                        .ToList()
                })
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
                        Message = m.MessageText

                    }).ToList();
                return messages;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;

            }
        }

        public bool SaveMessage(Message message)
        {
            try
            {
                message.DateSent = DateTime.UtcNow;
                _dataContext.Messages.Add(message);
                return _dataContext.SaveChanges() > 0;
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        
        }
    }
}
