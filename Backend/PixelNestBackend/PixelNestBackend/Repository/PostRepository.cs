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
  
        public PostRepository(
            DataContext dataContext,
            IConfiguration configuration
            )
        {
            _dataContext = dataContext;
            _configuration = configuration;
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
             
                return new PostResponse
                {
                    IsSuccessfull = false,
                    Message = $"Database error: {ex.Message}"
                };
            }
            catch (Exception ex)
            {
                // Log general exceptions (e.g. _logger.LogError(ex, "An unexpected error occurred"))
                return new PostResponse
                {
                    IsSuccessfull = false,
                    Message = $"An unexpected error occurred: {ex.Message}"
                };
            }
        }

        public async Task<ICollection<Post>> GetPosts()
        {
            var posts = await _dataContext.Posts.Select(a => new Post
            {
                PostDescription = a.PostDescription,
                OwnerUsername = a.OwnerUsername,
                TotalComments = a.TotalComments,
                TotalLikes = a.TotalLikes,
                PostID = a.PostID,
                PublishDate = a.PublishDate,
                ImagePaths = a.ImagePaths,
                Comments = a.Comments.Select(c => new Comment
                {
                    CommentText = c.CommentText,
                    TotalLikes = c.TotalLikes,
                    UserID = c.UserID,
                    Username = c.Username
                }).ToList(),
                LikedByUsers = a.LikedPosts.Select(l => new LikeDto {
                    Username = l.Username

                }).ToList()


            }).ToListAsync();

            return posts;
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

                Console.WriteLine("Sql related error: ", ex.Message);
                return false;
            }
            catch(Exception ex)
            {
                Console.WriteLine("General error: ", ex.Message);
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
                Console.WriteLine(ex.Message);
                return false;
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
