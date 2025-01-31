using Microsoft.AspNetCore.Mvc;
using PixelNestBackend.Dto;
using PixelNestBackend.Dto.Projections;
using PixelNestBackend.Models;
using PixelNestBackend.Responses;

namespace PixelNestBackend.Interfaces
{
    public interface IPostRepository
    {
        Task<PostResponse> PublishPost(PostDto postDto, int userID);
        Task<ICollection<ResponsePostDto>> GetPosts(string username);
        Task<ICollection<ResponsePostDto>> GetPostsByUsername(string username,string currentLoggedUser);
        Task<ICollection<ResponsePostDto>> GetPostsByLocation(string location, string username);
        bool SavePost(int userID, SavePostDto savePostDto, bool isDuplicate);
        PostResponse LikePost(LikeDto likeDto, bool isLiked, int userID);
        PostResponse Comment(Comment comment);
        Task<DeleteResponse> DeletePost(int postID);
        int ExtractUserID(int postID);
        bool CheckIntegrity(int userID, string email);
    }
}
