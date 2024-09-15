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
         

            string newPostQuery = "INSERT INTO Posts(UserID, OwnerUsername, PostDescription, TotalComments, TotalLikes, PublishDate) Values(@UserID, @OwnerUsername, @PostDescription, @TotalComments, @TotalLikes, GETDATE())";
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            if(postDto != null)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(newPostQuery, connection))
                    {
                        command.Parameters.AddWithValue("@UserID", userID);
                        command.Parameters.AddWithValue("@OwnerUsername", postDto.OwnerUsername);
                        command.Parameters.AddWithValue("@PostDescription", postDto.PostDescription);
                        command.Parameters.AddWithValue("@TotalComments", 0);
                        command.Parameters.AddWithValue("@TotalLikes", 0);
                        connection.Open();
                        int i = command.ExecuteNonQuery();

                        string getPostID = "SELECT @@IDENTITY";
                        using (SqlCommand getPostIDCommand = new SqlCommand(getPostID, connection))
                        {
                            int postID = Convert.ToInt32(getPostIDCommand.ExecuteScalar());
                            
                            connection.Close();
                            if (i > 0)
                            {
                                return new PostResponse {
                                    IsSuccessfull = true,
                                    PostID = postID,
                                    Message = "Post was successfully added"
                                    
                                    
                                };
                            }
                            else return new PostResponse
                            {
                                IsSuccessfull = false,
                                Message = "Post was not added"
                            };
                        }
                    }

                }
            }
            return new PostResponse
            {
                IsSuccessfull = false,
                Message = "PostDto is null!"
            };
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
    }
}
