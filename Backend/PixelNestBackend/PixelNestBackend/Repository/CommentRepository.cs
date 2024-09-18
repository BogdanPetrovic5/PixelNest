using Microsoft.Data.SqlClient;
using PixelNestBackend.Data;
using PixelNestBackend.Dto;
using PixelNestBackend.Interfaces;
using PixelNestBackend.Models;

namespace PixelNestBackend.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly DataContext _dataContext;
        public readonly ILogger<CommentRepository> _logger;
        public CommentRepository(
                DataContext dataContext,
                ILogger<CommentRepository> logger
            )
        {
            _dataContext = dataContext;
            _logger = logger;
        }
        public bool LikeComment(int userID, int commentID)
        {
            try
            {
                var likeObj = new LikedComments{
                   UserID = userID,
                   CommentID = commentID
                };
              
                
                _dataContext.LikeComments.Add(likeObj);
                return _dataContext.SaveChanges() > 0;
                
               
            }catch(SqlException ex)
            {
                _logger.LogError($"SQL related error: {ex.Message}");
                return false;
                
            }
            catch(Exception ex)
            {
                _logger.LogError($"General error: {ex.Message}");
                return false;
            }
        }
    }
}
