using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PixelNestBackend.Dto;
using PixelNestBackend.Dto.Projections;
using PixelNestBackend.Interfaces;
using PixelNestBackend.Responses;
using PixelNestBackend.Services.Menagers;
using PixelNestBackend.Utility;
using System.Security.Claims;

namespace PixelNestBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {

        private readonly IUserService _userService;
        private readonly UserUtility _userUtility;
        private readonly WebSocketConnectionMenager _webSocketConnection;
        public UserController(IUserService userService, WebSocketConnectionMenager webSocketConnectionMenager, UserUtility userUtility)
        {
            _userService = userService;
            _webSocketConnection = webSocketConnectionMenager;
            _userUtility = userUtility;
        }
  
        [Authorize]
        [HttpPost("CloseConnection")]
        public async Task<ActionResult> CloseConnectionWithSocket()
        {
            string? email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (email == null) return Unauthorized();
            int userID = _userService.GetUserID(email);
           string username=  _userUtility.GetUserName(userID);
           await _webSocketConnection.CloseConnection(username);
            return Ok();
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
        [Authorize]
        [HttpPut("ChangeProfilePicture")]
        public async Task<ActionResult<bool>> ChangePicture([FromForm] ProfileDto profileDto)
        {
            
            string? email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (email == null) return Unauthorized();

            bool response = await this._userService.ChangePicture(profileDto, email);
            if (response) return Ok(new { message = "Profile picture changed successfully" });
            return NotFound();
            
        }
        [Authorize]
        [HttpPut("ChangeUsername")]
        public ActionResult<bool> ChangeUsername([FromForm] ProfileDto profileDto)
        {
            string? email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (email == null) return Unauthorized();

            bool response = _userService.ChangeUsername(email, profileDto.Username);

            if (response) return Ok(new { message = "Username changed successfully"} );
            return NotFound(new { message = "Error!" });
            
        }
        [Authorize]
        [HttpGet("GetProfilePicture")]
        public ActionResult<string> GetProfilePicture(string username)
        {
            string? pictureUrl = _userService.GetPicture(username);
            if(pictureUrl != null)
            {
                return Ok(new {path = pictureUrl });
            }
            return NotFound(new { path = string.Empty });
            
        }
      
        [HttpGet("FindUsers")]
        public ActionResult<ICollection<ResponseUsersDto>> FindUsers(string username)
        {
            if(username.IsNullOrEmpty()) return BadRequest(string.Empty);
            ICollection<ResponseUsersDto> responseUsers = _userService.FindUsers(username);
            if(responseUsers != null)
            {
                return Ok(responseUsers);
            }else return NotFound();
        }
    
    } 
}
