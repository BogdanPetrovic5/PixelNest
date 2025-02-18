using Microsoft.AspNetCore.Mvc;
using PixelNestBackend.Dto;
using PixelNestBackend.Dto.Projections;
using PixelNestBackend.Models;
using PixelNestBackend.Responses;

namespace PixelNestBackend.Interfaces
{
    public interface IPostRepository
    {
        Task<PostResponse> PublishPost(PostDto postDto, Guid userID);
        Task<ICollection<ResponsePostDto>> GetPosts(string username);
        Task<ICollection<ResponsePostDto>> GetPostsByUsername(string username,string currentLoggedUser);
        Task<ICollection<ResponsePostDto>> GetPostsByLocation(string location, string username);
        Task<ResponsePostDto> GetSinglePost(Guid postID, string currentLoggedUser);
        bool SavePost(Guid userID, SavePostDto savePostDto, bool isDuplicate);
        bool CacheChange(string username);
        PostResponse LikePost(LikeDto likeDto, bool isLiked, Guid userID);
        PostResponse Comment(Comment comment);
        Task<DeleteResponse> DeletePost(Guid postID);
        Guid ExtractUserID(Guid postID);
        bool CheckIntegrity(Guid userID, string email);
    }
}
