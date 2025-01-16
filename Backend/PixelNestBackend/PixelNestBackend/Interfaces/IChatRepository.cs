using PixelNestBackend.Dto;
using PixelNestBackend.Dto.Projections;
using PixelNestBackend.Models;
using PixelNestBackend.Responses;

namespace PixelNestBackend.Interfaces
{
    public interface IChatRepository
    {
        bool SaveMessage(Message message, bool isUserInRoom);
        ICollection<ResponseMessagesDto> GetUserToUserMessages(int userID, int targetID);
        ICollection<ResponseChatsDto> GetUserChats(int userID);
        bool MarkAsRead(MarkAsRead markAsrReadDto, int userID);
        int GetNumberOfNewMessages(int userID);
    }
}
