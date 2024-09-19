using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PixelNestBackend.Data;
using PixelNestBackend.Dto;
using PixelNestBackend.Interfaces;
using PixelNestBackend.Models;
using PixelNestBackend.Responses;
using PixelNestBackend.Services;
using PixelNestBackend.Utility;

namespace PixelNestBackend.Repository
{
    public class PostRepository : IPostRepository
    {
        private readonly DataContext _dataContext;
        private readonly IConfiguration _configuration;
        private readonly ILogger<PostRepository> _logger;

        public PostRepository(
            DataContext dataContext,
            IConfiguration configuration,
            ILogger<PostRepository> logger
            )
        {
            _dataContext = dataContext;
            _configuration = configuration;
            _logger = logger;
        }
        public async Task<PostResponse> ShareNewPost(PostDto postDto, string userFolderPath, int userID)
        {
            if (postDto == null)
            {
                return new PostResponse
                {
                    IsSuccessfull = false,
                    Message = "PostDto is null!"
                };
            }

            string newPostQuery = @"INSERT INTO Posts(UserID, OwnerUsername, PostDescription, TotalComments, TotalLikes, PublishDate) 
                            VALUES(@UserID, @OwnerUsername, @PostDescription, @TotalComments, @TotalLikes, GETDATE());
                            SELECT CAST(SCOPE_IDENTITY() as int)";
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    using (SqlCommand command = new SqlCommand(newPostQuery, connection))
                    {
                      
                        command.Parameters.AddWithValue("@UserID", userID);
                        command.Parameters.AddWithValue("@OwnerUsername", postDto.OwnerUsername);
                        command.Parameters.AddWithValue("@PostDescription", postDto.PostDescription);
                        command.Parameters.AddWithValue("@TotalComments", 0);
                        command.Parameters.AddWithValue("@TotalLikes", 0);

                      
                        int postID = (int)await command.ExecuteScalarAsync();

                        return new PostResponse
                        {
                            IsSuccessfull = true,
                            PostID = postID,
                            Message = "Post was successfully added"
                        };
                    }
                }
            }
            catch (SqlException ex)
            {
                _logger.LogError($"Database error: {ex.Message}");
                return new PostResponse
                {
                    IsSuccessfull = false,
                    Message = $"Database error: {ex.Message}"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"General error: {ex.Message}");
                return new PostResponse
                {
                    IsSuccessfull = false,
                    Message = $"An unexpected error occurred: {ex.Message}"
                };
            }
        }

        public async Task<ICollection<ResponsePostDto>> GetPosts()
        {
            try
            {
             var posts = await _dataContext.Posts
                .Select(a => new ResponsePostDto
                {
                    PostDescription = a.PostDescription,
                    OwnerUsername = a.OwnerUsername,
                    TotalComments = a.TotalComments,
                    TotalLikes = a.TotalLikes,
                    PostID = a.PostID,
                    PublishDate = a.PublishDate,
                    ImagePaths = a.ImagePaths,


                    AllComments = a.Comments != null ? a.Comments.Where(c => c.ParentCommentID == null).Select(c => new ResponseCommentDto
                    {
                        CommentID = c.CommentID,
                        CommentText = c.CommentText,
                        TotalLikes = c.TotalLikes,
                        UserID = c.UserID,
                        Username = c.Username,
                        PostID = c.PostID,
                        ParentCommentID = c.ParentCommentID,
                        LikedByUsers = c.LikedComments != null
                        ? c.LikedComments.Select(l => new LikeCommentDto
                        {
                            Username = l.Username
                        }).ToList()
                        : new List<LikeCommentDto>(),
                        Replies = _GetReplies(c.CommentID, a.Comments)
                        

                    }).ToList() : new List<ResponseCommentDto>(),

                  
                    LikedByUsers = a.LikedPosts.Select(l => new LikeDto
                    {
                        Username = l.Username

                    }).ToList()

                })
                .AsSplitQuery() 
                .ToListAsync();

                _logger.LogInformation("Posts retrieved successfully.");
                return posts;
            }
            catch (SqlException ex)
            {
                _logger.LogError($"Database error while retrieving posts: {ex.Message}");
                throw;
            }
            catch (Exception ex) 
            {
                _logger.LogError($"An unexpected error occurred: {ex.Message}");
                throw; 
            }

        }
        public bool LikePost(LikeDto likeDto, bool isLiked, int userID)
        {
           
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            string query;
            if (!isLiked)
            {
                query = "INSERT INTO LikedPosts (UserID, PostID, Username, DateLiked) Values(@UserID, @PostID, @Username, GETDATE())";
            }
            else query = "DELETE FROM LikedPosts WHERE PostID = @PostID AND UserID = @UserID";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        command.Parameters.AddWithValue("@UserID", userID);
                        command.Parameters.AddWithValue("@PostID", likeDto.PostID);
                        command.Parameters.AddWithValue("@Username", likeDto.Username);
                        int i = command.ExecuteNonQuery();
                        connection.Close();
                        if (i > 0)
                        {

                            return true;
                        }
                        else return false;
                    }
                }
            }catch(SqlException ex)
            {

                _logger.LogError($"Database related error: {ex.Message}");
                return false;
            }
            catch(Exception ex)
            {
                _logger.LogError($"General error: {ex.Message}");
                return false;
            }
           


        } 
        public bool Comment(Comment comment)
        {
            try
            {
                _dataContext.Comments.Add(comment);
                int result = _dataContext.SaveChanges();
                if (result > 0) return true;
                return false;
                       
            }catch(SqlException ex)
            {
                _logger.LogError($"Database related error: {ex.Message}");
                return false;
            }catch(Exception ex)
            {
                _logger.LogError($"General error in repo: {ex.Message}");
                return false;
            }
        }
    
        private static List<ResponseCommentDto> _GetReplies(int commentID, ICollection<Comment> allComments)
        {
            return allComments != null ? allComments.Where(reply => reply.ParentCommentID == commentID)
                .Select(reply => new ResponseCommentDto
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

                }).ToList() : new List<ResponseCommentDto>();
        }
    }
}
