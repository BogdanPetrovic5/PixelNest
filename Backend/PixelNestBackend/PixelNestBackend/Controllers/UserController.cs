using Microsoft.AspNetCore.Mvc;
using PixelNestBackend.Dto;
using PixelNestBackend.Dto.Projections;
using PixelNestBackend.Interfaces;
using PixelNestBackend.Responses;

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
        [HttpGet("GetFollowings")]
        public ICollection<ResponseFollowingDto>? GetFollowings(string username)
        {
            ICollection<ResponseFollowingDto> users = _userRepository.GetFollowings(username);
            if (users != null)
            {
                return users;
            }
            else return null;
        }
        [HttpGet("GetFollowers")]
        public ICollection<ResponseFollowersDto>? GetFollowers(string username)
        {
            ICollection<ResponseFollowersDto> users = _userRepository.GetFollowers(username);
            if (users != null)
            {
                return users;
            }
            else return null;
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
        public UserProfileDto? GetUserData(string username)
        {
            
            UserProfileDto user = _userRepository.GetUserProfileData(username);

            if(user != null)
            {
                return user;
            }
            return null;
        }

        [HttpGet("IsFollowing")]
        public ActionResult<bool> IsFollowing([FromQuery]FollowDto followDto)
        {
            FollowResponse followResponse = _userRepository.IsFollowing(followDto);

            if (followResponse == null) return BadRequest();

            if (followResponse.IsSuccessful)
            {
                return Ok(followResponse.IsFollowing);
            }
            else return NotFound();
        }
    }
}
