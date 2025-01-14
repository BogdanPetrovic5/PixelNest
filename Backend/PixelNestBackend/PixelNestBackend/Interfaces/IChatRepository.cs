using PixelNestBackend.Dto.Projections;
using PixelNestBackend.Models;
using PixelNestBackend.Responses;

namespace PixelNestBackend.Interfaces
{
    public interface IChatRepository
    {
        bool SaveMessage(Message message);
        ICollection<ResponseMessagesDto> GetUserToUserMessages(int userID, int targetID);
        ICollection<ResponseChatsDto> GetUserChats(int userID);
    }
}
