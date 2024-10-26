using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PixelNestBackend.Dto;
using PixelNestBackend.Dto.Projections;
using PixelNestBackend.Interfaces;
using PixelNestBackend.Models;
using PixelNestBackend.Responses;

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
        [HttpPost("PublishPost")]
        public async Task<ActionResult<PostResponse>> PublishPost([FromForm] PostDto postDto)
        {
            if(postDto == null)
            {
                return BadRequest(new PostResponse { IsSuccessfull = false, Message = "PostDto has bad request body!" });
            }
            try
            {

                var result = await _postService.PublishPost(postDto);

                if (result.IsSuccessfull)
                {
                    return Ok(new PostResponse { Message = result.Message, IsSuccessfull = true });
                }
                return NotFound(new PostResponse { Message = result.Message, IsSuccessfull = false });
            }catch (Exception ex)
            {
                return StatusCode(500, new PostResponse { Message = ex.Message, IsSuccessfull = false });
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
        public async Task<ActionResult<ICollection<ResponsePostDto>>> GetPosts(string? username,string? location, int page = 1, int maximumPosts = 5){
            try
            {
                ICollection<ResponsePostDto> posts;
           
                posts = await _postService.GetPosts(username,location);
                if (posts == null && !posts.Any()) return NotFound(new { message = "No posts found"});
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
        public ActionResult<PostResponse> Comment(CommentDto commentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            try
            {
                PostResponse result = _postService.Comment(commentDto);
                if (result.IsSuccessfull)
                {
                    return Ok(new PostResponse { Message = result.Message, IsSuccessfull = result.IsSuccessfull });
                }
                else return BadRequest(new PostResponse { Message = result.Message, IsSuccessfull = result.IsSuccessfull });
            }catch(Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
            
        }

       
    }
}
