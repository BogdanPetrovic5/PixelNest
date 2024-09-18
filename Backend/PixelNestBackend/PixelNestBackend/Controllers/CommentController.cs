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
