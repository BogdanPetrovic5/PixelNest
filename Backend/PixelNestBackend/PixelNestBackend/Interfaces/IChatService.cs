using PixelNestBackend.Dto;
using PixelNestBackend.Dto.Projections;
using PixelNestBackend.Responses;

namespace PixelNestBackend.Interfaces
{
    public interface IChatService
    {
        MessageResponse SaveMessage(MessageDto messageDto);
        ICollection<ResponseMessagesDto> GetUserToUserMessages(string username, string targetUsername);
        ICollection<ResponseChatsDto> GetUserChats(string email);
    }
}
