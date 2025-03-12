using PixelNestBackend.Dto;
using PixelNestBackend.Dto.Projections;
using PixelNestBackend.Models;
using PixelNestBackend.Responses;

namespace PixelNestBackend.Interfaces
{
    public interface IPostService
    {

        Task<PostResponse> PublishPost(PostDto postDto);
        Task<ICollection<ResponsePostDto>> GetPosts(string? clientGuid, string? location, string userGuid);
        Task<ResponsePostDto> GetSinglePost(Guid postID, string userGuid);
        bool SavePost(SavePostDto savePostDto);
        bool CacheChange(string userGuid);
        PostResponse LikePost(LikeDto likeDto);
        PostResponse Comment(CommentDto commentDto, string userGuid);
        Task<DeleteResponse> DeletePost(string userGuid, Guid postID);
        bool CheckIntegrity(string email, Guid postID);
    }
}
