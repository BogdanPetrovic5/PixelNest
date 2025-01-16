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

        public int GetNumberOfNewMessages(string email)
        {
            string username = _userUtility.GetUserName(email);
            int userID = _userUtility.GetUserID(username);

            int newMessages = _chatRepository.GetNumberOfNewMessages(userID);
            return newMessages;
        }

        public ICollection<ResponseChatsDto> GetUserChats(string email)
        {
            string username = _userUtility.GetUserName(email);
            int userID = _userUtility.GetUserID(username);
            return _chatRepository.GetUserChats(userID);
        }

        public ICollection<ResponseMessagesDto> GetUserToUserMessages(string username, string targetUsername)
        {
            int userID = _userUtility.GetUserID(username);
            int targetID = _userUtility.GetUserID(targetUsername);
            return _chatRepository.GetUserToUserMessages(userID, targetID);
        }

        public bool MarkAsRead(MarkAsRead markAsrReadDto,string email)
        {
            string username = _userUtility.GetUserName(email);
            int userID = _userUtility.GetUserID(username);

            return _chatRepository.MarkAsRead(markAsrReadDto, userID);
            throw new NotImplementedException();
        }

        public MessageResponse SaveMessage(MessageDto messageDto)
        {
            Message message = new Message();

            message.SenderID = _userUtility.GetUserID(messageDto.SenderUsername);
            message.ReceiverID = _userUtility.GetUserID(messageDto.ReceiverUsername);
            message.MessageText = messageDto.Message;

            string roomID = _connectionMenager.FindRoom(messageDto.ReceiverUsername, messageDto.SenderUsername);

            bool isUserInRoom = _connectionMenager.IsUserInRoom(roomID, messageDto.ReceiverUsername);
            bool response = _chatRepository.SaveMessage(message, isUserInRoom);

            return new MessageResponse { 
                IsSuccessfull = response,
                ReceiverID = message.ReceiverID, 
                SenderID = message.SenderID 
            };
        }
    }
}
