using Microsoft.AspNetCore.Mvc;
using PixelNestBackend.Dto;
using PixelNestBackend.Models;
using PixelNestBackend.Responses;

namespace PixelNestBackend.Interfaces
{
    public interface IPostRepository
    {
        Task<PostResponse> ShareNewPost(PostDto postDto, string userFolderPath, int userID);
        Task<ICollection<ResponsePostDto>> GetPosts();
        Task<ICollection<ResponsePostDto>> GetPostsByUsername(string username);
        Task<ICollection<ResponsePostDto>> GetPostsByLocation(string username);
        bool SavePost(int userID, SavePostDto savePostDto, bool isDuplicate);
        bool LikePost(LikeDto likeDto, bool isLiked, int userID);
        bool Comment(Comment comment);

    }
}
