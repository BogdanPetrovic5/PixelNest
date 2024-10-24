using Microsoft.AspNetCore.Mvc;
using PixelNestBackend.Dto;
using PixelNestBackend.Dto.Projections;
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
        public ActionResult<ICollection<ResponseReplyCommentDto>> GetReplies(int initialParentID)
        {
            ICollection<ResponseReplyCommentDto> result = _commentService.GetReplies(initialParentID);
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound();
        }
        [HttpGet("GetComments")]
        public ActionResult<ICollection<ResponseCommentDto>?> GetComments(int postID)
        {
            ICollection<ResponseCommentDto> result;
            result = _commentService.GetComments(postID);
            if (result != null)
            {
                return Ok(result);
            }
            else return NotFound();
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
