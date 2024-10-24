using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PixelNestBackend.Data;
using PixelNestBackend.Dto;
using PixelNestBackend.Dto.Projections;
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
        public  ICollection<ResponseReplyCommentDto> GetReplies(int? initialParentID)
        {

            try
            {
                var allComments = _dataContext.Comments.Include(c => c.LikedComments).ToList();
                var replies = allComments.Where(c => c.ParentCommentID == initialParentID).ToList();
                return replies.Select(r => new ResponseReplyCommentDto
                {
                    CommentID = r.CommentID,
                    CommentText = r.CommentText,
                    TotalLikes = r.TotalLikes,
                    UserID = r.UserID,
                    Username = r.Username,
                    PostID = r.PostID,
                    ParentCommentID = r.ParentCommentID,
                    TotalReplies = r.TotalReplies,
                    LikedByUsers = r.LikedComments != null
                             ? r.LikedComments.Select(l => new LikeCommentDto
                             {
                                 Username = l.Username
                             }).ToList()
                             : new List<LikeCommentDto>(),
                    Replies = _GetReplies(r.CommentID, allComments)


                }).ToList();
            }catch(Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }
        public ICollection<ResponseCommentDto> GetComments(int postID)
        {
            try
            {
                var allComments = _dataContext.Comments.Include(c => c.LikedComments).Where(c => c.PostID == postID).ToList();

          
                return allComments
                    .Where(c => c.ParentCommentID == null) 
                    .Select(c => new ResponseCommentDto
                    {
                        CommentID = c.CommentID,
                        CommentText = c.CommentText,
                        TotalLikes = c.TotalLikes,
                        UserID = c.UserID,
                        Username = c.Username,
                        PostID = c.PostID,
                        ParentCommentID = c.ParentCommentID,
                        TotalReplies = c.TotalReplies,
                        LikedByUsers = c.LikedComments != null
                            ? c.LikedComments.Select(l => new LikeCommentDto
                            {
                                Username = l.Username
                            }).ToList()
                            : new List<LikeCommentDto>(),
                        
                    })
                    .ToList();
            }
            catch(Exception ex)
            {
                return null;
            }
        }
        public bool LikeComment(int userID, LikeCommentDto likeCommentDto, bool isDuplicate)
        {
            try
            {
                var likeObj = new LikedComments
                {
                    UserID = userID,
                    CommentID = likeCommentDto.CommentID,
                    Username = likeCommentDto.Username

                };
                if (!isDuplicate)
                {
                    Console.WriteLine("Nije duplikat");


                    _dataContext.LikeComments.Add(likeObj);
                    return _dataContext.SaveChanges() > 0;
                }
                var existingLike = _dataContext.LikeComments
                    .FirstOrDefault(l => l.UserID == userID && l.CommentID == likeCommentDto.CommentID);

                if (existingLike != null)
                {
                    _dataContext.LikeComments.Remove(existingLike);
                    return _dataContext.SaveChanges() > 0;
                }
                else
                {
                    Console.WriteLine("No such like found to remove");
                    return false; 
                }

            }
            catch(SqlException ex)
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

        private static List<ResponseReplyCommentDto> _GetReplies(int commentID, ICollection<Comment> allComments)
        {
            return allComments != null ? allComments.Where(reply => reply.ParentCommentID == commentID)
                .Select(reply => new ResponseReplyCommentDto
                {
                    CommentID = reply.CommentID,
                    CommentText = reply.CommentText,
                    TotalLikes = reply.TotalLikes,
                    UserID = reply.UserID,
                    Username = reply.Username,
                    PostID = reply.PostID,
                    ParentCommentID = reply.ParentCommentID,
                    LikedByUsers = reply.LikedComments != null
                        ? reply.LikedComments.Select(l => new LikeCommentDto
                        {
                            Username = l.Username
                        }).ToList()
                        : new List<LikeCommentDto>(),


                    Replies = _GetReplies(reply.CommentID, allComments)

                }).ToList() : new List<ResponseReplyCommentDto>();
        }
    }
    
}
