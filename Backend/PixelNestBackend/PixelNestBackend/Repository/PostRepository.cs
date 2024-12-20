
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PixelNestBackend.Data;
using PixelNestBackend.Dto;
using PixelNestBackend.Dto.Projections;
using PixelNestBackend.Interfaces;
using PixelNestBackend.Models;
using PixelNestBackend.Responses;
using PixelNestBackend.Security;


namespace PixelNestBackend.Repository
{
    public class PostRepository : IPostRepository
    {
        private readonly DataContext _dataContext;
        private readonly IConfiguration _configuration;
        private readonly ILogger<PostRepository> _logger;
        private readonly SASTokenGenerator _SAStokenGenerator;
        public PostRepository(
            DataContext dataContext,
            IConfiguration configuration,
            ILogger<PostRepository> logger,
            SASTokenGenerator SASTokenGenerator
            )
        {
            _dataContext = dataContext;
            _configuration = configuration;
            _logger = logger;
            _SAStokenGenerator = SASTokenGenerator;
        }

        public bool SavePost(int userID, SavePostDto savePostDto, bool isDuplicate)
        {
            var obj = new SavedPosts
            {
                UserID = userID,
                PostID = savePostDto.PostID,
                Username = savePostDto.Username
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

            string newPostQuery = @"INSERT INTO Posts(UserID, OwnerUsername, PostDescription, TotalComments, TotalLikes, PublishDate, Location) 
                            VALUES(@UserID, @OwnerUsername, @PostDescription, @TotalComments, @TotalLikes, GETDATE(), @Location);
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
        public async Task<ICollection<ResponsePostDto>> GetPostsByUsername(string username)
        {
            try
            {
                ICollection<ResponsePostDto> posts = await _dataContext.Posts
                    .Where(u => u.OwnerUsername == username)
                   .Select(a => new ResponsePostDto
                   {
                       PostDescription = a.PostDescription,
                       OwnerUsername = a.OwnerUsername,
                       TotalComments = a.TotalComments,
                       TotalLikes = a.TotalLikes,
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
                           Username = l.Username

                       }).ToList(),
                       SavedByUsers = a.SavedPosts.Select(s => new SavePostDto
                       {
                           Username = s.Username

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
        public async Task<ICollection<ResponsePostDto>> GetPostsByLocation(string location)
        {
            try
            {
                ICollection<ResponsePostDto> posts = await _dataContext.Posts
                   .Where(l => l.Location.ToLower().Contains(location.ToLower()))
                   .Select(a => new ResponsePostDto
                   {
                       PostDescription = a.PostDescription,
                       OwnerUsername = a.OwnerUsername,
                       TotalComments = a.TotalComments,
                       TotalLikes = a.TotalLikes,
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
                           Username = l.Username

                       }).ToList(),
                       SavedByUsers = a.SavedPosts.Select(s => new SavePostDto
                       {
                           Username = s.Username

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
        public async Task<ICollection<ResponsePostDto>> GetPosts()
        {
            try
            {
                ICollection<ResponsePostDto> posts = await _dataContext.Posts
                   .Select(a => new ResponsePostDto
                   {
                       PostDescription = a.PostDescription,
                       OwnerUsername = a.OwnerUsername,
                       TotalComments = a.TotalComments,
                       TotalLikes = a.TotalLikes,
                       PostID = a.PostID,
                       PublishDate = a.PublishDate,
                       ImagePaths = a.ImagePaths.Select(l => new ResponseImageDto {
                           Path = l.Path,
                           PhotoDisplay = l.PhotoDisplay,
                           PathID = l.PathID

                       }).ToList(),
                       Location = a.Location,
                       LikedByUsers = a.LikedPosts.Select(l => new LikeDto
                       {
                           Username = l.Username

                       }).ToList(),
                       SavedByUsers = a.SavedPosts.Select(s => new SavePostDto
                       {
                           Username = s.Username

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
    
        
    }
}
