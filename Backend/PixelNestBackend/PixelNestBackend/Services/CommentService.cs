using PixelNestBackend.Dto;
using PixelNestBackend.Dto.Projections;
using PixelNestBackend.Interfaces;
using PixelNestBackend.Models;
using PixelNestBackend.Utility;

namespace PixelNestBackend.Services
{
    public class CommentService : ICommentService
    {
        private readonly UserUtility _userUtility;
        private readonly CommentUtility _commentUtility;
        private readonly ICommentRepository _commentRepository;
        public CommentService(
            UserUtility userUtility,
            ICommentRepository commentRepository,
            CommentUtility commentUtility
            
            )
        {
            _userUtility = userUtility;
            _commentRepository = commentRepository;
            _commentUtility = commentUtility;
        }
        public ICollection<ResponseReplyCommentDto> GetReplies(int? initialParentID)
        {
            ICollection<ResponseReplyCommentDto> result;
            result = _commentRepository.GetReplies(initialParentID);
            return result;
        }
        public ICollection<ResponseCommentDto> GetComments(Guid postID)
        {
            ICollection<ResponseCommentDto> result;
            result = _commentRepository.GetComments(postID);
            return result;
        }
        public bool LikeComment(int commentID, Guid userGuid)
        {
            
           
            
            bool isDuplicate = _commentUtility.FindDuplicates(userGuid, commentID);
            Console.WriteLine(isDuplicate);
            if (userGuid != Guid.Empty)
            {
                bool result = _commentRepository.LikeComment(userGuid, commentID, isDuplicate);
                if (result)
                {
                    return true;
                }
                else return false;
            }return false;
        }
    }
}
