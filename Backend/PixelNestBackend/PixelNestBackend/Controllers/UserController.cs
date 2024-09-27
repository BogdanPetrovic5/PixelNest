using Microsoft.AspNetCore.Mvc;
using PixelNestBackend.Dto;
using PixelNestBackend.Interfaces;

namespace PixelNestBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {

        private readonly IUserService _userRepository;
        public UserController(IUserService userRepository)
        {
            _userRepository = userRepository;
        }
        [HttpGet("GetUserProfile")]
        public IActionResult GetUserData(string username)
        {
            
            UserProfileDto user = _userRepository.GetUserProfileData(username);

            if(user != null)
            {
                return Ok(user);
            }
            return NotFound();
        }
    }
}
