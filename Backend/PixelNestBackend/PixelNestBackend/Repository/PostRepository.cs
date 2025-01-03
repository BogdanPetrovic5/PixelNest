﻿
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PixelNestBackend.Data;
using PixelNestBackend.Dto;
using PixelNestBackend.Dto.Projections;
using PixelNestBackend.Interfaces;
using PixelNestBackend.Models;
using PixelNestBackend.Responses;
using PixelNestBackend.Security;
using PixelNestBackend.Utility;


namespace PixelNestBackend.Repository
{
    public class PostRepository : IPostRepository
    {
        private readonly DataContext _dataContext;
        private readonly IConfiguration _configuration;
        private readonly ILogger<PostRepository> _logger;
        private readonly SASTokenGenerator _SAStokenGenerator;
        private readonly UserUtility _userUtility;
        public PostRepository(
            DataContext dataContext,
            IConfiguration configuration,
            ILogger<PostRepository> logger,
            SASTokenGenerator SASTokenGenerator,
            UserUtility userUtility
            )
        {
            _dataContext = dataContext;
            _configuration = configuration;
            _logger = logger;
            _SAStokenGenerator = SASTokenGenerator;
            _userUtility = userUtility;
        }


        public bool SavePost(int userID, SavePostDto savePostDto, bool isDuplicate)
        {
            var obj = new SavedPosts
            {
                UserID = userID,
                PostID = savePostDto.PostID,
                
            };
            if (!isDuplicate)
            {
                _dataContext.SavedPosts.Add(obj);
                return _dataContext.SaveChanges() > 0; 
            }
            var existingSave = _dataContext.SavedPosts.FirstOrDefault(sp => sp.PostID == savePostDto.PostID && sp.UserID == userID);
            if (existingSave != null) 
            {
                _dataContext.SavedPosts.Remove(existingSave);
                return _dataContext.SaveChanges() > 0;
            }
            return false;
        }
        public async Task<PostResponse> PublishPost(PostDto postDto,int userID)
        {
            if (postDto == null)
            {
                return new PostResponse
                {
                    IsSuccessfull = false,
                    Message = "PostDto is null!"
                };
            }

            string newPostQuery = @"INSERT INTO Posts(UserID, PostDescription, TotalComments, TotalLikes, PublishDate, Location) 
                            VALUES(@UserID, @PostDescription, @TotalComments, @TotalLikes, GETDATE(), @Location);
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
                       
                        
                        command.Parameters.AddWithValue("@TotalComments", 0);
                        command.Parameters.AddWithValue("@TotalLikes", 0);
                        if (string.IsNullOrEmpty(postDto.Location))
                        {
                            command.Parameters.AddWithValue("@Location", DBNull.Value); 
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@Location", postDto.Location);
                        }

                        if (string.IsNullOrEmpty(postDto.PostDescription))
                        {
                            command.Parameters.AddWithValue("@PostDescription", DBNull.Value);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@PostDescription", postDto.PostDescription);
                        }
                        int postID = (int)await command.ExecuteScalarAsync();
                        User user = _dataContext.Users.FirstOrDefault(u => u.Username == postDto.OwnerUsername);
                        if(user != null)
                        {
                            user.TotalPosts += 1;
                        }
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
        public async Task<ICollection<ResponsePostDto>> GetPostsByUsername(string username, string currentLoggedUser)
        {
            try
            {
                int userID = _userUtility.GetUserID(username);
                int currentLoggedUserID = _userUtility.GetUserID(currentLoggedUser);
                ICollection<ResponsePostDto> posts = await _dataContext.Posts
                    .Where(u => u.UserID == userID)
                   .Select(a => new ResponsePostDto
                   {
                       PostDescription = a.PostDescription,
                       OwnerUsername = a.User.Username,
                       TotalComments = a.TotalComments,
                       TotalLikes = a.TotalLikes,
                       PostID = a.PostID,
                       IsDeletable = a.UserID == currentLoggedUserID,
                       PublishDate = a.PublishDate,
                       ImagePaths = a.ImagePaths.Select(l => new ResponseImageDto
                       {
                           Path = l.Path,
                           PhotoDisplay = l.PhotoDisplay,
                           PathID = l.PathID

                       }).ToList(),
                       Location = a.Location,
                       LikedByUsers = a.LikedPosts.Select(l => new LikeDto
                       {
                           Username = l.User.Username

                       }).ToList(),
                       SavedByUsers = a.SavedPosts.Select(s => new SavePostDto
                       {
                           Username = s.User.Username

                       }).ToList()
                   })
                .AsSplitQuery()
                .ToListAsync();
                foreach (var post in posts)
                {
                    _SAStokenGenerator.appendSasToken(post.ImagePaths);
                }
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
        public async Task<ICollection<ResponsePostDto>> GetPostsByLocation(string location, string username)
        {
            try
            {
                
                int userID = _userUtility.GetUserID(username);
                ICollection<ResponsePostDto> posts = await _dataContext.Posts
                   .Where(l => l.Location.ToLower().Contains(location.ToLower()))
                   .Select(a => new ResponsePostDto
                   {
                       PostDescription = a.PostDescription,
                       OwnerUsername = a.User.Username,
                       TotalComments = a.TotalComments,
                       TotalLikes = a.TotalLikes,
                       IsDeletable = a.UserID == userID,
                       PostID = a.PostID,
                       PublishDate = a.PublishDate,
                       ImagePaths = a.ImagePaths.Select(l => new ResponseImageDto
                       {
                           Path = l.Path,
                           PhotoDisplay = l.PhotoDisplay,
                           PathID = l.PathID

                       }).ToList(),
                       Location = a.Location,
                       LikedByUsers = a.LikedPosts.Select(l => new LikeDto
                       {
                           Username = l.User.Username

                       }).ToList(),
                       SavedByUsers = a.SavedPosts.Select(s => new SavePostDto
                       {
                           Username = s.User.Username

                       }).ToList()
                   })
                .AsSplitQuery()
                .ToListAsync();
                foreach (var post in posts)
                {
                    _SAStokenGenerator.appendSasToken(post.ImagePaths);
                }

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
        public async Task<ICollection<ResponsePostDto>> GetPosts(string username)
        {
            try
            {
                
                ICollection<ResponsePostDto> posts = await _dataContext.Posts
                   .Select(a => new ResponsePostDto
                   {
                       PostDescription = a.PostDescription,
                      
                       TotalComments = a.TotalComments,
                       TotalLikes = a.TotalLikes,
                       OwnerUsername = a.User.Username,
                       PostID = a.PostID,
                       IsDeletable = a.User.Username == username,
                       PublishDate = a.PublishDate,
                       ImagePaths = a.ImagePaths.Select(l => new ResponseImageDto {
                           Path = l.Path,
                           PhotoDisplay = l.PhotoDisplay,
                           PathID = l.PathID

                       }).ToList(),
                       Location = a.Location,
                       LikedByUsers = a.LikedPosts.Select(l => new LikeDto
                       {
                           Username = l.User.Username

                       }).ToList(),
                       SavedByUsers = a.SavedPosts.Select(s => new SavePostDto
                       {
                           Username = s.User.Username

                       }).ToList()
                   })
                .AsSplitQuery() 
                .ToListAsync();
                foreach (var post in posts)
                {
                    _SAStokenGenerator.appendSasToken(post.ImagePaths);
                }
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
                query = "INSERT INTO LikedPosts (UserID, PostID, DateLiked) Values(@UserID, @PostID,GETDATE())";
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
        public PostResponse Comment(Comment comment)
        {
            try
            {
                var post = _dataContext.Posts.FirstOrDefault(p => p.PostID == comment.PostID);
                if (post != null)
                {
                   
                    post.TotalComments += 1;
                }

                _dataContext.Comments.Add(comment);
                if (comment.ParentCommentID.HasValue)
                {
                    var parentComment = _dataContext.Comments.FirstOrDefault(c => c.CommentID == comment.ParentCommentID.Value && c.ParentCommentID == null);
                    if (parentComment != null)
                    {
                        parentComment.TotalReplies += 1;
                    }
                }

                int result = _dataContext.SaveChanges();

                if (result > 0) return new PostResponse {Message = "Comment Added!", IsSuccessfull = true };
                return new PostResponse { Message = "Failed to add comment!", IsSuccessfull = false };

            }
            catch(SqlException ex)
            {
                _logger.LogError($"Database related error: {ex.Message}");
                return null;
            }catch(Exception ex)
            {
                _logger.LogError($"General error in repo: {ex.Message}");
                return null;
            }
        }

        public async Task<DeleteResponse> DeletePost(int postID)
        {
            try
            {
                var post = _dataContext.Posts.FirstOrDefault(a => a.PostID == postID);
                var userID = _dataContext.Posts
                    .Where(p => p.PostID == postID)
                    .Select(p => p.UserID)
                    .FirstOrDefault();
                if (post == null)
                {
                    return new DeleteResponse
                    {
                        IsSuccess = false,
                        IsValid = true,
                        Message = "No post found!"
                    };
                }
                
                _dataContext.Comments.Where(a => a.PostID == postID).ExecuteDelete();
                _dataContext.LikedPosts.Where(a => a.PostID == postID).ExecuteDelete();
                _dataContext.SavedPosts.Where(a => a.PostID == postID).ExecuteDelete();
                _dataContext.ImagePaths.Where(a => a.PostID == postID).ExecuteDelete();
                User? user = _dataContext.Users.FirstOrDefault(u => u.UserID == userID);
                if (user != null) user.TotalPosts -= 1;
                _dataContext.Posts.Remove(post);
                
                await _dataContext.SaveChangesAsync();

                return new DeleteResponse
                {
                    IsSuccess = true,
                    IsValid = true,
                    Message = "Successfuly deleted!"
                };

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new DeleteResponse
                {
                    IsSuccess = false,
                    IsValid = true,
                    Message = "Server failed!"
                };
            }
        }

        public int ExtractUserID(int postID)
        {
            int userID = _dataContext.Posts.Where(a => a.PostID == postID)
                .Select(userID => userID.UserID).FirstOrDefault();

            return userID;
        }

        public bool CheckIntegrity(int userID, string email)
        {
            try
            {
                bool isValid = _dataContext.Users.Where(u => u.UserID == userID).Any(e => e.Email == email);
                return isValid;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
