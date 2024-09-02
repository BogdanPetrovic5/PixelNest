using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using PixelNestBackend.Data;
using PixelNestBackend.Dto;
using PixelNestBackend.Interfaces;
using PixelNestBackend.Services;
using PixelNestBackend.Utility;

namespace PixelNestBackend.Repository
{
    public class PostRepository : IPostRepository
    {
        private readonly DataContext _dataContext;
        private readonly IConfiguration _configuration;
        private readonly string _basedFolderPath;
        private readonly FolderGenerator _folderGenerator;
        private readonly IPostService _postService;
        private readonly UserUtility _userUtility;
        public PostRepository(
            DataContext dataContext,
            FolderGenerator folderGenerator,
            IConfiguration configuration,
            IPostService postService,
            UserUtility userUtility
            )
        {
            _dataContext = dataContext;
            _basedFolderPath = Path.Combine("wwwroot", "Photos");
            _folderGenerator = folderGenerator;
            _configuration = configuration;
            _postService = postService;
            _userUtility = userUtility;
        }
        public async Task<bool> ShareNewPost(PostDto postDto)
        {
            string userFolderName = postDto.OwnerUsername;
            string userFolderPath = Path.Combine(_basedFolderPath, userFolderName);
            int userID = _userUtility.GetUserID(postDto.OwnerUsername);
            if (!_folderGenerator.CheckIfFolderExists(userFolderPath))
            {
                _folderGenerator.GenerateNewFolder(userFolderPath);

            }
            string newPostQuery = "INSERT INTO Posts(UserID, OwnerUsername, PostDescription, TotalComments, TotalLikes) Values(@UserID, @OwnerUsername, @PostDescription, @TotalComments, @TotalLikes)";
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            if(postDto.Photos != null)
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
                            await _postService.StoreImages(postDto, userFolderPath, postID);

                            if (i > 0)
                            {
                                return true;
                            }
                            else return false;
                        }
                    }

                }
            }
            return false;
        }
    }
}
