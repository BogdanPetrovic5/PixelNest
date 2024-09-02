using Microsoft.AspNetCore.Mvc;
using PixelNestBackend.Dto;

namespace PixelNestBackend.Interfaces
{
    public interface IPostRepository
    {
       Task<bool> ShareNewPost(PostDto postDto);
    }
}
