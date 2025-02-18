using PixelNestBackend.Dto;
using PixelNestBackend.Dto.Projections;
using PixelNestBackend.Models;
using PixelNestBackend.Responses;

namespace PixelNestBackend.Interfaces
{
    public interface IChatRepository
    {
        bool SaveMessage(Message message, bool isUserInRoom);
        ICollection<ResponseMessagesDto> GetUserToUserMessages(Guid userID, Guid targetID);
        ICollection<ResponseChatsDto> GetUserChats(Guid userID);
        bool MarkAsRead(MarkAsRead markAsrReadDto, Guid userID);
        int GetNumberOfNewMessages(Guid userID);
    }
}
