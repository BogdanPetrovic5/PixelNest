using PixelNestBackend.Dto;
using PixelNestBackend.Models;
using PixelNestBackend.Responses;

namespace PixelNestBackend.Interfaces
{
    public interface IPostService
    {

        Task<PostResponse> ShareNewPost(PostDto postDto);
        Task<ICollection<ResponsePostDto>> GetPosts();
        Task<ICollection<ResponsePostDto>> GetPostsByUsername(string username);
        bool SavePost(SavePostDto savePostDto);
        bool LikePost(LikeDto likeDto);
        bool Comment(CommentDto commentDto);
        
    }
}
