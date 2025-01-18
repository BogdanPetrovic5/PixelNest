using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
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
        private readonly IMemoryCache _memoryCache;
        private const string CommentsCacheKey = "Comments_{0}";
        private readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(10);

        public CommentRepository(
                DataContext dataContext,
                ILogger<CommentRepository> logger,
                IMemoryCache memoryCache
            )
        {
            _dataContext = dataContext;
            _logger = logger;
            _memoryCache = memoryCache;
        }
        public ICollection<ResponseReplyCommentDto> GetReplies(int? initialParentID)
        {

            try
            {
                var cacheKey = string.Format(CommentsCacheKey, "Replies" + initialParentID);
                Console.WriteLine(cacheKey);
                if (!_memoryCache.TryGetValue(cacheKey, out ICollection<ResponseReplyCommentDto> cachedReplies))
                {
                    var allComments = _dataContext.Comments
                        .Include(c => c.LikedComments)
                        .Include(u => u.User)
                        .ToList();
                    var replies = allComments
                        .Where(c => c.ParentCommentID == initialParentID)
                        .ToList();

                    cachedReplies = replies.Select(r => new ResponseReplyCommentDto
                    {
                        CommentID = r.CommentID,
                        CommentText = r.CommentText,
                        TotalLikes = r.TotalLikes,
                        UserID = r.UserID,
                        Username = r.User.Username,
                        PostID = r.PostID,
                        ParentCommentID = r.ParentCommentID,
                        TotalReplies = r.TotalReplies,
                        LikedByUsers = r.LikedComments != null
                             ? r.LikedComments.Select(l => new LikeCommentDto
                             {
                                 Username = l.User.Username
                             }).ToList()
                             : new List<LikeCommentDto>(),
                        Replies = _GetReplies(r.CommentID, allComments)


                    }).ToList();
                    _memoryCache.Set(cacheKey, cachedReplies, new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
                    });
                }
                return cachedReplies;


                //var allComments = _dataContext.Comments.Include(c => c.LikedComments).Include(u => u.User).ToList();
                //var replies = allComments.Where(c => c.ParentCommentID == initialParentID).ToList();
                //return replies.Select(r => new ResponseReplyCommentDto
                //{
                //    CommentID = r.CommentID,
                //    CommentText = r.CommentText,
                //    TotalLikes = r.TotalLikes,
                //    UserID = r.UserID,
                //    Username = r.User.Username,
                //    PostID = r.PostID,
                //    ParentCommentID = r.ParentCommentID,
                //    TotalReplies = r.TotalReplies,
                //    LikedByUsers = r.LikedComments != null
                //             ? r.LikedComments.Select(l => new LikeCommentDto
                //             {
                //                 Username = l.User.Username
                //             }).ToList()
                //             : new List<LikeCommentDto>(),
                //    Replies = _GetReplies(r.CommentID, allComments)


                //}).ToList();
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
                var cacheKey = string.Format(CommentsCacheKey, postID);
                Console.WriteLine(cacheKey);
                if (!_memoryCache.TryGetValue(cacheKey, out ICollection<ResponseCommentDto> cachedComments)){
                    var allComments = _dataContext.Comments.Include(c => c.LikedComments).Include(c => c.User).Where(c => c.PostID == postID).ToList();
                    cachedComments = allComments
                    .Where(c => c.ParentCommentID == null)
                    .Select(c => new ResponseCommentDto
                    {
                        CommentID = c.CommentID,
                        CommentText = c.CommentText,
                        TotalLikes = c.TotalLikes,
                        UserID = c.UserID,
                        Username = c.User != null ? c.User.Username : "Unknown",
                        PostID = c.PostID,
                        ParentCommentID = c.ParentCommentID,
                        TotalReplies = c.TotalReplies,
                        LikedByUsers = c.LikedComments != null
                            ? c.LikedComments.Select(l => new LikeCommentDto
                            {
                                Username = l.User.Username
                            }).ToList()
                            : new List<LikeCommentDto>(),

                    })
                    .ToList();
                    _memoryCache.Set(cacheKey, cachedComments, new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30) // Cache for 30 minutes
                    });
                }
                return cachedComments;
                //var allComments = _dataContext.Comments.Include(c => c.LikedComments).Include(c => c.User).Where(c => c.PostID == postID).ToList();

          
                //return allComments
                //    .Where(c => c.ParentCommentID == null) 
                //    .Select(c => new ResponseCommentDto
                //    {
                //        CommentID = c.CommentID,
                //        CommentText = c.CommentText,
                //        TotalLikes = c.TotalLikes,
                //        UserID = c.UserID,
                //        Username = c.User != null ? c.User.Username : "Unknown",
                //        PostID = c.PostID,
                //        ParentCommentID = c.ParentCommentID,
                //        TotalReplies = c.TotalReplies,
                //        LikedByUsers = c.LikedComments != null
                //            ? c.LikedComments.Select(l => new LikeCommentDto
                //            {
                //                Username = l.User.Username
                //            }).ToList()
                //            : new List<LikeCommentDto>(),
                        
                //    })
                //    .ToList();
            }
            catch(Exception ex)
            {
                Console.WriteLine("Exception: "+ ex.Message);
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
                  

                };
                if (!isDuplicate)
                {
                    


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
                    Username = reply.User.Username,
                    PostID = reply.PostID,
                    ParentCommentID = reply.ParentCommentID,
                    LikedByUsers = reply.LikedComments != null
                        ? reply.LikedComments.Select(l => new LikeCommentDto
                        {
                            Username = l.User.Username
                        }).ToList()
                        : new List<LikeCommentDto>(),


                    Replies = _GetReplies(reply.CommentID, allComments)

                }).ToList() : new List<ResponseReplyCommentDto>();
        }
    }
    
}
