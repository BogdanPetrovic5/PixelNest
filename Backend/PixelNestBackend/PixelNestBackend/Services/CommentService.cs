using PixelNestBackend.Dto;
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
        public ICollection<ResponseCommentDto> GetComments(int postID)
        {
            ICollection<ResponseCommentDto> result;
            result = _commentRepository.GetComments(postID);
            return result;
        }
        public bool LikeComment(LikeCommentDto likeCommentDto)
        {
            int userID = _userUtility.GetUserID(likeCommentDto.Username);
            Console.WriteLine(likeCommentDto.CommentID);
            Console.WriteLine(userID);
            
            bool isDuplicate = _commentUtility.FindDuplicates(userID, likeCommentDto.CommentID);
            Console.WriteLine(isDuplicate);
            if (userID > -1)
            {
                bool result = _commentRepository.LikeComment(userID, likeCommentDto, isDuplicate);
                if (result)
                {
                    return true;
                }
                else return false;
            }return false;
        }
    }
}
