using Microsoft.AspNetCore.Mvc;
using PixelNestBackend.Dto;
using PixelNestBackend.Interfaces;

namespace PixelNestBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;
        public CommentController(
                ICommentService commentService
            
            )
        {
            _commentService = commentService;
        }
        [HttpGet("GetReplies")]
        public IActionResult GetReplies(int initialParentID)
        {
            var result = _commentService.GetReplies(initialParentID);
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest();
        }
        [HttpGet("GetComments")]
        public IActionResult GetComments(int postID)
        {
            ICollection<ResponseCommentDto> result;
            result = _commentService.GetComments(postID);
            if (result != null)
            {
                return Ok(result);
            }
            else return BadRequest();
        }
        [HttpPost("LikeComment")]
        public IActionResult Like(LikeCommentDto likeCommentDto)
        {
            if(likeCommentDto == null)
            {
                return BadRequest();
            }
            bool result = _commentService.LikeComment(likeCommentDto);
            if (result)
            {
                return Ok(new { Message = "Successfully liked comment!" });
            }
            return NotFound();
        }
    }
}
