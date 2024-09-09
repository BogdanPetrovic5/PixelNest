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
            try
            {

                var result = await _postRepository.ShareNewPost(postDto);
                if (result)
                {
                    return Ok(new { message = "Post was successfully added to your Nest feed" });
                }
                return BadRequest(result);
            }catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving posts.", error = ex.Message });
            }
           
          
        }
        [HttpGet("GetPosts")]
        public async Task<IActionResult> GetPosts(int page = 1, int maximumPosts = 16){
            try
            {
                ICollection<Post> posts;
                posts = await _postRepository.GetPosts();
                if(posts == null && !posts.Any()) return NotFound(new { message = "No posts found"});
                var result  = posts.OrderByDescending(a => a.PublishDate);
                return Ok(result);
            }
            catch(Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving posts.", error = ex.Message });
            }
          

           
        }
    }
}
