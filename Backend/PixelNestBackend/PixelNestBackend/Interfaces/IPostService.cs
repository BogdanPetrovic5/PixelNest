using PixelNestBackend.Dto;
using PixelNestBackend.Models;
using PixelNestBackend.Responses;

namespace PixelNestBackend.Interfaces
{
    public interface IPostService
    {

        Task<PostResponse> ShareNewPost(PostDto postDto);
        Task<ICollection<ResponsePostDto>> GetPosts();

        bool LikePost(LikeDto likeDto);
        bool Comment(CommentDto commentDto);

    }
}
