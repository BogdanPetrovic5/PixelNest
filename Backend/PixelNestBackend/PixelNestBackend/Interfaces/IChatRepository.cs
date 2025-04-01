using PixelNestBackend.Dto;
using PixelNestBackend.Dto.Projections;
using PixelNestBackend.Models;
using PixelNestBackend.Responses;

namespace PixelNestBackend.Interfaces
{
    public interface IChatRepository
    {
        bool SaveMessage(Message message, bool isUserInRoom);
        ICollection<ResponseMessagesDto> GetUserToUserMessages(string chatID, Guid userID);
        ICollection<ResponseChatsDto> GetUserChats(string userGuid);
        bool MarkAsRead(MarkAsRead markAsrReadDto, string userGuid);
        int GetNumberOfNewMessages(string userGuid);
        ICollection<ResponseChatsDto> SearchChats(string parameter, string userGuid);
    }
}
