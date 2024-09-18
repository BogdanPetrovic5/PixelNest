using PixelNestBackend.Dto;

namespace PixelNestBackend.Interfaces
{
    public interface ICommentService
    {
        bool LikeComment(LikeCommentDto likeCommentDto);
    }
}
