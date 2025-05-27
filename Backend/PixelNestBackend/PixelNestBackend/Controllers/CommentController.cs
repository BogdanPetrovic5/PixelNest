using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PixelNestBackend.Dto;
using PixelNestBackend.Dto.Projections;
using PixelNestBackend.Interfaces;
using System.Security.Claims;

namespace PixelNestBackend.Controllers
{
    [Route("api/comment")]
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
        [Authorize]
        [HttpGet("replies")]
        public ActionResult<ICollection<ResponseReplyCommentDto>> GetReplies(int initialParentID)
        {
            ICollection<ResponseReplyCommentDto> result = _commentService.GetReplies(initialParentID);
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound();
        }
        [Authorize]
        [HttpGet("comments")]
        public ActionResult<ICollection<ResponseCommentDto>?> GetComments(Guid postID)
        {
            ICollection<ResponseCommentDto> result;
            result = _commentService.GetComments(postID);
            if (result != null)
            {
                return Ok(result);
            }
            else return NotFound();
        }
        [Authorize]
        [HttpPost("{commentID}/like")]
        public IActionResult Like(int commentID)
        {
            string? userGuid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userGuid is null) return Unauthorized();
          
            bool result = _commentService.LikeComment(commentID, Guid.Parse(userGuid));
            if (result)
            {
                return Ok(new { Message = "Successfully liked comment!" });
            }
            return NotFound();
        }
    }
}
