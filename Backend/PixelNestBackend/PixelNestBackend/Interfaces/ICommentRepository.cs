using PixelNestBackend.Dto;

namespace PixelNestBackend.Interfaces
{
    public interface ICommentRepository
    {
        bool LikeComment(int userID, LikeCommentDto likeCommentDto);
    }
}
