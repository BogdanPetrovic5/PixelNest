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
        [HttpPost("Follow")]
        public IActionResult Follow(FollowDto followDto)
        {
            if (followDto == null) return BadRequest();
            bool response = _userRepository.Follow(followDto);
            if (response)
            {
                return Ok();
            }
            else return NotFound();
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
