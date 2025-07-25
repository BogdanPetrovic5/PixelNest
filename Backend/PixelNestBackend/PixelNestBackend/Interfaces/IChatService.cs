using PixelNestBackend.Dto;
using PixelNestBackend.Dto.Projections;
using PixelNestBackend.Responses;

namespace PixelNestBackend.Interfaces
{
    public interface IChatService
    {
        MessageResponse SaveMessage(MessageDto messageDto, string userGuid);
        ICollection<ResponseMessagesDto> GetUserToUserMessages(string chatID, string userID);
        ICollection<ResponseChatsDto> GetUserChats(string userGuid);
        bool MarkAsRead(MarkAsRead markAsrReadDto, string userGuid);
        int GetNumberOfNewMessages(string userGuid);
        ICollection<ResponseChatsDto> SearchChats(string parameter, string userGuid);
        bool DeleteForMe(int messageID, string userGuid);
        Task<MessageResponse> Unsend(int messageID, string userGuid);
    }
}
