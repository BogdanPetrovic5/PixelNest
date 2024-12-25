using PixelNestBackend.Dto;
using PixelNestBackend.Dto.Projections;
using PixelNestBackend.Responses;

namespace PixelNestBackend.Interfaces
{
    public interface ICommentRepository
    {
        ICollection<ResponseCommentDto> GetComments(int postID);
        ICollection<ResponseReplyCommentDto> GetReplies(int? initialParentID);
        bool LikeComment(int userID, LikeCommentDto likeCommentDto, bool isDuplicate);

        //Task<DeleteResponse> DeleteComment(int commentID);
        //string ExtractUsername(int commentID);
        //bool CheckIntegrity(string username, string email);
    }
}
