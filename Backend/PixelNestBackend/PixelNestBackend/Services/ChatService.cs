using PixelNestBackend.Dto;
using PixelNestBackend.Dto.Projections;
using PixelNestBackend.Interfaces;
using PixelNestBackend.Models;
using PixelNestBackend.Responses;
using PixelNestBackend.Utility;

namespace PixelNestBackend.Services
{
    public class ChatService : IChatService
    {
        private readonly UserUtility _userUtility;
        private readonly IChatRepository _chatRepository;
        public ChatService(UserUtility userUtility, IChatRepository chatRepository) { 
            _userUtility = userUtility;
            _chatRepository = chatRepository;
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

        public MessageResponse SaveMessage(MessageDto messageDto)
        {
            Message message = new Message();

            message.SenderID = _userUtility.GetUserID(messageDto.SenderUsername);
            message.ReceiverID = _userUtility.GetUserID(messageDto.ReceiverUsername);
            message.MessageText = messageDto.Message;
            
            bool response = _chatRepository.SaveMessage(message);

            return new MessageResponse { 
                IsSuccessfull = response,
                ReceiverID = message.ReceiverID, 
                SenderID = message.SenderID 
            };
        }
    }
}
