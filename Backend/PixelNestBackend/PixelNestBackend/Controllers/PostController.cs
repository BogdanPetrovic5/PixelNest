using Microsoft.AspNetCore.Mvc;
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
        public IActionResult ShareNewPost(string email)
        {
            var result = _postRepository.ShareNewPost(email);
            if (result)
            {
                return Ok();
            }
            return BadRequest();
        }
    }
}
