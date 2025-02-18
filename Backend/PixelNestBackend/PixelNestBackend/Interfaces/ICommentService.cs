using PixelNestBackend.Dto;
using PixelNestBackend.Dto.Projections;

namespace PixelNestBackend.Interfaces
{
    public interface ICommentService
    {
        bool LikeComment(LikeCommentDto likeCommentDto);
        ICollection<ResponseCommentDto> GetComments(Guid postID);
        ICollection<ResponseReplyCommentDto> GetReplies(int? initialParentID);
    }
}
