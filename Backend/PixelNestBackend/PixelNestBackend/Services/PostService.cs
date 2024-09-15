using Microsoft.Extensions.Configuration;
using PixelNestBackend.Data;
using PixelNestBackend.Dto;
using PixelNestBackend.Interfaces;
using PixelNestBackend.Models;
using PixelNestBackend.Repository;
using PixelNestBackend.Responses;
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
       
        public PostService(
            
            UserUtility userUtility,
            PostUtility postUtility,
            FolderGenerator folderGenerator,
            IPostRepository postRepository,
            IFileUpload fileUpload
            )
        {
           
            _userUtility = userUtility;
            _postUtility = postUtility; 
            _basedFolderPath = Path.Combine("wwwroot", "Photos");
            _folderGenerator = folderGenerator;
            _postRepository = postRepository;
            _fileUpload = fileUpload;
        }

        public async Task<ICollection<Post>> GetPosts()
        {
            var result = await _postRepository.GetPosts();
            return result;
        }

        public bool LikePost(LikeDto likeDto)
        {
            int userID = _userUtility.GetUserID(likeDto.Username);
            bool isLiked = _postUtility.FindDuplicate(likeDto.PostID, userID);
            
            bool result = _postRepository.LikePost(likeDto, isLiked, userID);
            if (result) return true;
            return false;
        }

        public async Task<PostResponse> ShareNewPost(PostDto postDto)
        {
            string userFolderName = postDto.OwnerUsername;
            string userFolderPath = Path.Combine(_basedFolderPath, userFolderName);
            int userID = _userUtility.GetUserID(postDto.OwnerUsername);

            if (!_folderGenerator.CheckIfFolderExists(userFolderPath))
            {
                _folderGenerator.GenerateNewFolder(userFolderPath);

            }
            var response = await _postRepository.ShareNewPost(postDto, userFolderPath, userID);

            if(response != null)
            {
                if (response.IsSuccessfull)
                {
                    int postID = response.PostID;
                    bool isUploaded = await _fileUpload.StoreImages(postDto, userFolderPath, postID);
                    if (isUploaded) return new PostResponse { 
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

      


    }
}
