using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PixelNestBackend.Dto;
using PixelNestBackend.Dto.Projections;
using PixelNestBackend.Interfaces;
using PixelNestBackend.Responses;
using System.Security.Claims;

namespace PixelNestBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {

        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet("GetFollowings")]
        public ICollection<ResponseFollowingDto>? GetFollowings(string username)
        {
            ICollection<ResponseFollowingDto> users = _userService.GetFollowings(username);
            if (users != null)
            {
                return users;
            }
            else return null;
        }
        [HttpGet("GetFollowers")]
        public ICollection<ResponseFollowersDto>? GetFollowers(string username)
        {
            ICollection<ResponseFollowersDto> users = _userService.GetFollowers(username);
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
            bool response = _userService.Follow(followDto);
            if (response)
            {
                return Ok();
            }
            else return NotFound();
        }
        [HttpGet("GetUserProfile")]
        public UserProfileDto? GetUserData(string username)
        {
            
            UserProfileDto user = _userService.GetUserProfileData(username);

            if(user != null)
            {
                return user;
            }
            return null;
        }

        [HttpGet("IsFollowing")]
        public ActionResult<bool> IsFollowing([FromQuery]FollowDto followDto)
        {
            FollowResponse followResponse = _userService.IsFollowing(followDto);

            if (followResponse == null) return BadRequest();

            if (followResponse.IsSuccessful)
            {
                return Ok(followResponse.IsFollowing);
            }
            else return NotFound();
        }
    
    } 
}
