using PixelNestBackend.Dto;
using PixelNestBackend.Dto.Projections;
using PixelNestBackend.Interfaces;
using PixelNestBackend.Models;
using PixelNestBackend.Responses;
using PixelNestBackend.Services.Menagers;
using PixelNestBackend.Utility;
using System.Reflection;

namespace PixelNestBackend.Services
{
    public class ChatService : IChatService
    {
        private readonly UserUtility _userUtility;
        private readonly IChatRepository _chatRepository;
        private readonly WebSocketConnectionMenager _connectionMenager;
        public ChatService(UserUtility userUtility, IChatRepository chatRepository, WebSocketConnectionMenager webSocketConnectionMenager) { 
            _userUtility = userUtility;
            _chatRepository = chatRepository;
            _connectionMenager = webSocketConnectionMenager;
        }

        public int GetNumberOfNewMessages(string userGuid)
        {
     
            int newMessages = _chatRepository.GetNumberOfNewMessages(userGuid);
            return newMessages;
        }

        public ICollection<ResponseChatsDto> GetUserChats(string userGuid)
        {
            
            return _chatRepository.GetUserChats(userGuid);
        }

        public ICollection<ResponseMessagesDto> GetUserToUserMessages(string chatID, string userGuid)
        {
            Guid userID = Guid.Parse(userGuid);
         
            return _chatRepository.GetUserToUserMessages(chatID, userID);
        }

        public bool MarkAsRead(MarkAsRead markAsrReadDto,string userGuid)
        {
       

            return _chatRepository.MarkAsRead(markAsrReadDto, userGuid);
           
        }

        public MessageResponse SaveMessage(MessageDto messageDto, string userGuid)
        {
            Message message = new Message();


            Guid senderClientGuid = Guid.Parse(_userUtility.GetClientGuid(userGuid));

            Guid receiverGuid = _userUtility.GetUserID(messageDto.ClientGuid);

            message.MessageText = messageDto.Message;
            message.SenderGuid = Guid.Parse(userGuid);
            message.ReceiverGuid = receiverGuid;


            message.ChatID = senderClientGuid.CompareTo(Guid.Parse(messageDto.ClientGuid)) < 0
                         ? $"{senderClientGuid.ToString()}-{messageDto.ClientGuid}"
                         : $"{messageDto.ClientGuid}-{senderClientGuid.ToString()}";

            string roomID = _connectionMenager.FindRoom(messageDto.ClientGuid, senderClientGuid.ToString());

            bool isUserInRoom = _connectionMenager.IsUserInRoom(roomID, messageDto.ClientGuid);
            bool response = _chatRepository.SaveMessage(message, isUserInRoom);

            return new MessageResponse { 
                IsSuccessfull = response,
                ReceiverID = Guid.Parse(_userUtility.GetClientGuid(message.ReceiverGuid.ToString())), 
                SenderID = Guid.Parse(_userUtility.GetClientGuid(message.SenderGuid.ToString())),
                Message = message.MessageText,
                ReceiverUsername = _userUtility.GetUserName(message.ReceiverGuid),
                SenderUsername = _userUtility.GetUserName(message.SenderGuid),
                
            };
        }

        public ICollection<ResponseChatsDto> SearchChats(string parameter, string userGuid)
        {
           return _chatRepository.SearchChats(parameter, userGuid);
        }
    }
}
