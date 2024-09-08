using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PixelNestBackend.Dto;
using PixelNestBackend.Interfaces;
using PixelNestBackend.Models;

namespace PixelNestBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostRepository _postRepository;
      
        public PostController(IPostRepository postRepository)
        {
            _postRepository = postRepository;

        }
        [Authorize]
        [HttpPost("ShareNewPost")]
        public async Task<IActionResult> ShareNewPost([FromForm] PostDto postDto)
        {
            var result = await _postRepository.ShareNewPost(postDto);
            if (result)
            {
                return Ok(new { message = "Post was successfully added to your Nest feed" });
            }
            return BadRequest("There was an error with publishing the post!");
        }
        [HttpGet("GetPosts")]
        public async Task<IActionResult> GetPosts(int page = 1, int maximumPosts = 16){
            ICollection<Post> posts;
            posts = await _postRepository.GetPosts();
            return Ok(posts);

           
        }
    }
}
