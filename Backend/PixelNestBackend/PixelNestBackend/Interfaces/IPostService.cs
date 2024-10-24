using PixelNestBackend.Dto;
using PixelNestBackend.Dto.Projections;
using PixelNestBackend.Models;
using PixelNestBackend.Responses;

namespace PixelNestBackend.Interfaces
{
    public interface IPostService
    {

        Task<PostResponse> PublishPost(PostDto postDto);
        Task<ICollection<ResponsePostDto>> GetPosts(string? username, string? location);
   
        bool SavePost(SavePostDto savePostDto);
        bool LikePost(LikeDto likeDto);
        PostResponse Comment(CommentDto commentDto);
        
    }
}
