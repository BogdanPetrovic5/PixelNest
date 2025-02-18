using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PixelNestBackend.Dto;
using PixelNestBackend.Dto.Projections;
using PixelNestBackend.Dto.WebSockets;
using PixelNestBackend.Interfaces;
using PixelNestBackend.Models;
using PixelNestBackend.Responses;
using PixelNestBackend.Services.Menagers;
using System.Security.Claims;

namespace PixelNestBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
      
        private readonly IPostService _postService;
        private readonly WebSocketConnectionMenager _webSocketConnectionMenager;
        public PostController(IPostService postService, WebSocketConnectionMenager webSocketConnectionMenager)
        {
            
            _postService = postService;
            _webSocketConnectionMenager = webSocketConnectionMenager;

        }
        [Authorize]
        [HttpGet("CheckCache")]
        public ActionResult<bool> CacheChange()
        {
            string? email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (email == null) return Unauthorized();
            bool response = _postService.CacheChange(email);
            if (response)
            {
                return Ok(true);
            }
            else return Ok(false);


        }
        [Authorize]
        [HttpGet("GetPost")]
        public async Task<ActionResult<ResponsePostDto>> GetPost(Guid postID)
        {
            string? email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (email == null) return Unauthorized();
            ResponsePostDto result = await _postService.GetSinglePost(postID, email);
            if(result == null) return NotFound();
            return Ok(result);
        }
        [Authorize]
        [HttpDelete("DeletePost")]
        public async Task<ActionResult<DeleteResponse>> DeletePost(Guid postID)
        {
            try
            {
                string? email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (email == null) return BadRequest();


                DeleteResponse deleteResponse = await _postService.DeletePost(email, postID);

                if(deleteResponse == null) return NotFound();
                
                if(!deleteResponse.IsValid) return Unauthorized(new DeleteResponse { IsSuccess = deleteResponse.IsSuccess, IsValid = deleteResponse.IsValid, Message = deleteResponse.Message });
               
                if (deleteResponse.IsValid && deleteResponse.IsSuccess)
                {
                    return Ok();
                }

                if (deleteResponse.IsValid && !deleteResponse.IsSuccess)
                {
                    return NotFound(new DeleteResponse { IsValid = deleteResponse.IsValid, IsSuccess = deleteResponse.IsSuccess, Message = deleteResponse.Message });
                }
                return NotFound(new DeleteResponse { IsSuccess = false, IsValid = false, Message = "Unknown error occured!" });
            }
            catch (Exception ex)
            {
                return NotFound();
            }
            

           
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
        [Authorize]
        [HttpPost("SavePost")]
        public IActionResult SavePost(SavePostDto savePostDto)
        {
            if (savePostDto == null) return BadRequest();
            bool result = _postService.SavePost(savePostDto);
            if (result) return Ok();
            return NotFound();
        }

        [Authorize]
        [HttpGet("GetPosts")]
        public async Task<ActionResult<ICollection<ResponsePostDto>>> GetPosts(string? username,string? location, int page = 1, int maximumPosts = 5){
            try
            {
                ICollection<ResponsePostDto> posts;
                string? userEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                posts = await _postService.GetPosts(username,location, userEmail);
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
        [Authorize]
        [HttpPost("LikePost")]
        public async Task<ActionResult> LikePost(LikeDto likeDto)
        {
            if(likeDto == null)
            {
                return BadRequest();
            }
            PostResponse likeResponse = null;
            likeResponse = _postService.LikePost(likeDto);
            if (likeResponse != null && likeResponse.IsSuccessfull)
            {
                WebSocketMessage webSocketMessage = new WebSocketMessage
                {
                    Content = "New Like",
                    SenderUsername = likeResponse.User,
                    TargetUser = likeResponse.TargetUser,
                    Type = "Like"
                };
                if (!likeResponse.DoubleAction && webSocketMessage.SenderUsername != webSocketMessage.TargetUser) await this._webSocketConnectionMenager.SendNotificationToUser(webSocketMessage);

                return Ok();
            } else return NotFound();
        }
        [Authorize]
        [HttpPost("Comment")]
        public async Task<ActionResult> Comment(CommentDto commentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            try
            {
                PostResponse commentResponse = _postService.Comment(commentDto);
                if (commentResponse.IsSuccessfull)
                {
                    WebSocketMessage webSocketMessage = new WebSocketMessage
                    {
                        Content = "New Comment",
                        SenderUsername = commentResponse.User,
                        TargetUser = commentResponse.TargetUser,
                        Type = "Comment"
                        
                    };
                    await this._webSocketConnectionMenager.SendNotificationToUser(webSocketMessage);
                    return Ok(new PostResponse { Message = commentResponse.Message, IsSuccessfull = commentResponse.IsSuccessfull });
                }
                else return BadRequest(new PostResponse { Message = commentResponse.Message, IsSuccessfull = commentResponse.IsSuccessfull });
            }catch(Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
            
        }

      

       
    }
}
