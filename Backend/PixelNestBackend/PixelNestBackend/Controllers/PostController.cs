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
        private readonly IPostService _postService;
      
        public PostController(IPostRepository postRepository, IPostService postService)
        {
            _postRepository = postRepository;
            _postService = postService;

        }
        [Authorize]
        [HttpPost("ShareNewPost")]
        public async Task<IActionResult> ShareNewPost([FromForm] PostDto postDto)
        {
            try
            {

                var result = await _postService.ShareNewPost(postDto);
                if (result.IsSuccessfull)
                {
                    return Ok(new { message = result.Message });
                }
                return BadRequest(new {message = result.Message});
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
                posts = await _postService.GetPosts();
                if(posts == null && !posts.Any()) return NotFound(new { message = "No posts found"});
                var result = posts.OrderByDescending(a => a.PublishDate);
                return Ok(result);
            }
            catch(Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving posts.", error = ex.Message });
            }
        }

        [HttpPost("LikePost")]
        public IActionResult LikePost(LikeDto likeDto)
        {
            bool result = _postService.LikePost(likeDto);
            if (result)
            {
                return Ok();
            } else return BadRequest();
        }
        [HttpPost("Comment")]
        public IActionResult Comment(CommentDto commentDto)
        {
            bool result = _postService.Comment(commentDto);
            if (result)
            {
                return Ok(new { message = "Comment successfully added!" });
            }
            else return BadRequest(new { message = "Comment successfully not added" });
        }
    }
}
