using PixelNestBackend.Dto;
using PixelNestBackend.Dto.Projections;

namespace PixelNestBackend.Interfaces
{
    public interface ICommentService
    {
        bool LikeComment(LikeCommentDto likeCommentDto);
        ICollection<ResponseCommentDto> GetComments(int postID);
        ICollection<ResponseReplyCommentDto> GetReplies(int? initialParentID);
    }
}
