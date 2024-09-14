using Microsoft.AspNetCore.Mvc;
using PixelNestBackend.Dto;
using PixelNestBackend.Models;

namespace PixelNestBackend.Interfaces
{
    public interface IPostRepository
    {
        Task<bool> ShareNewPost(PostDto postDto);
        Task<ICollection<Post>> GetPosts();

        bool LikePost(LikeDto likeDto);

    }
}
