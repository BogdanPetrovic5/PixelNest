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

        [HttpPost("SavePost")]
        public IActionResult SavePost(SavePostDto savePostDto)
        {
            if (savePostDto == null) return BadRequest();
            bool result = _postService.SavePost(savePostDto);
            if (result) return Ok();
            return NotFound();
        }
        [HttpGet("GetPosts")]
        public async Task<ActionResult<ICollection<ResponsePostDto>>> GetPosts(int page = 1, int maximumPosts = 5){
            try
            {
                ICollection<ResponsePostDto> posts;
                posts = await _postService.GetPosts();
                if(posts == null && !posts.Any()) return NotFound(new { message = "No posts found"});
                var result = posts.OrderByDescending(a => a.PublishDate);
                var paginatedPosts = result
                    .Skip((page - 1) * maximumPosts)  
                    .Take(maximumPosts)               
                    .ToList();
                
                return Ok(paginatedPosts);
            }
            catch(Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving posts.", error = ex.Message });
            }
        }

        [HttpPost("LikePost")]
        public IActionResult LikePost(LikeDto likeDto)
        {
            if(likeDto == null)
            {
                return BadRequest();
            }
            bool result = _postService.LikePost(likeDto);
            if (result)
            {
                return Ok();
            } else return NotFound();
        }
        [HttpPost("Comment")]
        public IActionResult Comment(CommentDto commentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            try
            {
                bool result = _postService.Comment(commentDto);
                if (result)
                {
                    return Ok(new { message = "Comment successfully added!" });
                }
                else return BadRequest(new { message = "Comment successfully not added" });
            }catch(Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
            
        }

        [HttpGet("GetPosts/ByLocation/{location}")]
        public async Task<ActionResult<ICollection<ResponsePostDto>>> GetPostsByLocation(string location, int page, int maximumPosts = 5)
        {
            ICollection<ResponsePostDto> posts = await _postService
                .GetPostsByLocation(location);

            if(posts == null)
            {
                return NotFound();
            }
            var result = posts.OrderByDescending(a => a.PublishDate);
            var paginatedPosts = result
                .Skip((page - 1) * maximumPosts)
                .Take(maximumPosts)
                .ToList();
            return Ok(paginatedPosts);

        }

        [HttpGet("GetPosts/ByUsername/{username}")]
        public async Task<ActionResult<ICollection<ResponsePostDto>>> GetPostsByUsername(string username, int page, int maximumPosts = 5)
        {
            ICollection<ResponsePostDto> posts = await _postService
                .GetPostsByUsername(username);
            if(posts != null)
            {
                var result = posts.OrderByDescending(a => a.PublishDate);
                var paginatedPosts = result
                    .Skip((page - 1) * maximumPosts)
                    .Take(maximumPosts)
                    .ToList();

                return Ok(paginatedPosts);
            }
            return NotFound();
        }
    }
}
