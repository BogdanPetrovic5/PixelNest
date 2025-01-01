using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using PixelNestBackend.Data;
using PixelNestBackend.Dto;
using PixelNestBackend.Dto.Projections;
using PixelNestBackend.Gateaway;
using PixelNestBackend.Interfaces;
using PixelNestBackend.Models;
using PixelNestBackend.Repository;
using PixelNestBackend.Responses;
using PixelNestBackend.Security;
using PixelNestBackend.Utility;

namespace PixelNestBackend.Services
{
    public class PostService : IPostService
    {
  
        private readonly UserUtility _userUtility;
        private readonly PostUtility _postUtility;
        private readonly FolderGenerator _folderGenerator;
        private readonly IPostRepository _postRepository;
        public readonly IFileUpload _fileUpload;
        private readonly string _basedFolderPath;
        private readonly ILogger<PostService> _ILogger;
       
        public PostService(

            UserUtility userUtility,
            PostUtility postUtility,
            FolderGenerator folderGenerator,
           
            IPostRepository postRepository,
            IFileUpload fileUpload,
            ILogger<PostService> logger
            )
        {
           
            _userUtility = userUtility;
            _postUtility = postUtility; 
            _basedFolderPath = Path.Combine("wwwroot", "Photos");
            _folderGenerator = folderGenerator;
            _postRepository = postRepository;
            _fileUpload = fileUpload;
            _ILogger = logger;
          
        }
        public bool SavePost(SavePostDto savePostDto)
        {
            int userID = _userUtility.GetUserID(savePostDto.Username);
            if (userID < 1) return false;

            bool isLiked = _postUtility.FindDuplicate(savePostDto.PostID, userID, "savedPosts");

            bool result = _postRepository.SavePost(userID, savePostDto, isLiked);
            if (result) return true;
            return false;
        }
        public async Task<ICollection<ResponsePostDto>> GetPosts(string? username, string? location, string email)
        {
            ICollection<ResponsePostDto> posts;
            string currentLoggedUser = _userUtility.GetUserName(email);
            if (!string.IsNullOrEmpty(username) && string.IsNullOrEmpty(location))
            {
                posts = await _postRepository.GetPostsByUsername(username, currentLoggedUser);
            }
            else if (string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(location))
            {
                posts = await _postRepository.GetPostsByLocation(location,currentLoggedUser);
            }
            else
            {
                posts = await _postRepository.GetPosts(currentLoggedUser);
            }
            return posts;
        }

        public bool LikePost(LikeDto likeDto)
        {
            int userID = _userUtility.GetUserID(likeDto.Username);
            if (userID < 0) return false;
            bool isLiked = _postUtility.FindDuplicate(likeDto.PostID, userID, "likedPosts");
            
            bool result = _postRepository.LikePost(likeDto, isLiked, userID);
            if (result) return true;
            return false;
        }

        public async Task<PostResponse> PublishPost(PostDto postDto)
        {
            int userID = _userUtility.GetUserID(postDto.OwnerUsername);
            string userFolderName = userID.ToString();

            string userFolderPath = Path.Combine(_basedFolderPath, userFolderName);
            

            if (!_folderGenerator.CheckIfFolderExists(userFolderPath))
            {
                _folderGenerator.GenerateNewFolder(userFolderPath);

            }
            var response = await _postRepository
                .PublishPost(postDto,userID);

            if(response != null)
            {
                if (response.IsSuccessfull)
                {
                    int postID = response.PostID;
                    bool isUploaded = await _fileUpload.StoreImages(postDto, null,null, userFolderPath, postID, null);
                    //bool isUploadedBlob = await _blobStorageUpload.StoreImages(postDto, null, postDto.OwnerUsername, postID);
                    //if (isUploadedBlob) return new PostResponse
                    //{
                    //    IsSuccessfull = true,
                    //    Message = "Post was successfully added to your feed."
                    //};
                    if (isUploaded) return new PostResponse
                    {
                        IsSuccessfull = true,
                        Message = "Post was successfully added to your feed."
                    };
                    return new PostResponse
                    {
                        IsSuccessfull = false,
                        Message = "Images failed to upload."
                    };
                }
                return new PostResponse
                {
                    IsSuccessfull = false,
                    Message = "Post failed to upload."
                };
            }
            return new PostResponse
            {
                IsSuccessfull = false,
                Message = "Internal server error."
            }; 
        }

        public PostResponse Comment(CommentDto commentDto)
        {
            try
            {
                int userID = _userUtility.GetUserID(commentDto.Username);
                if (userID < 0)
                {
                    Console.WriteLine("user not found");
                    return new PostResponse {Message = "User not found!", IsSuccessfull = false };
                }
                Comment comment = new Comment
                {
                    UserID = userID,
                    CommentText = commentDto.CommentText,
                    
                    PostID = commentDto.PostID,
                    TotalLikes = 0,
                    ParentCommentID = commentDto.ParentCommentID
                    
                };
                return _postRepository.Comment(comment);
            }
            catch (SqlException ex)
            {
                _ILogger.LogError($"Database error: {ex.Message}");
                return new PostResponse { Message = "Database error", IsSuccessfull = false };
            }
            catch (Exception ex)
            {
                _ILogger.LogError($"General error in service: {ex.Message}");
                return new PostResponse { Message = "Unknown error occurred", IsSuccessfull = false };
            }
           
        }
        public bool CheckIntegrity(string email, int postID)
        {

            int userID = _postRepository.ExtractUserID(postID);
            if (userID != null) { 
                bool isValid = _postRepository.CheckIntegrity(userID, email);
                if (!isValid)
                {
                    return false;
                }return true;
            }return false;
        }
        public async Task<DeleteResponse> DeletePost(string email, int postID)
        {
            if (CheckIntegrity(email, postID))
            {
                return await _postRepository.DeletePost(postID);
            }
            return new DeleteResponse { IsSuccess = false, IsValid = false, Message = "You do not have authority to do this!" };
        }
    }
}
