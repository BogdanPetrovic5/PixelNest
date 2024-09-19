using PixelNestBackend.Dto;
using PixelNestBackend.Interfaces;
using PixelNestBackend.Utility;

namespace PixelNestBackend.Services
{
    public class CommentService : ICommentService
    {
        private readonly UserUtility _userUtility;
        private readonly ICommentRepository _commentRepository;
        public CommentService(
            UserUtility userUtility,
            ICommentRepository commentRepository
            
            )
        {
            _userUtility = userUtility;
            _commentRepository = commentRepository;
        }
        public bool LikeComment(LikeCommentDto likeCommentDto)
        {
            int userID = _userUtility.GetUserID(likeCommentDto.Username);
            if(userID > -1)
            {
                bool result = _commentRepository.LikeComment(userID, likeCommentDto);
                if (result)
                {
                    return true;
                }
                else return false;
            }return false;
        }
    }
}
