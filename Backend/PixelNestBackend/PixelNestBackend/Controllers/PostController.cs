using Microsoft.AspNetCore.Mvc;
using PixelNestBackend.Dto;
using PixelNestBackend.Interfaces;

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

        [HttpPost("ShareNewPost")]
        public async Task<IActionResult> ShareNewPost([FromForm] PostDto postDto)
        {
            var result = await _postRepository.ShareNewPost(postDto);
            if (result)
            {
                return Ok("Post was successfully added to your Nest feed!");
            }
            return BadRequest("There was an error with publishing the post!");
        }
    }
}
