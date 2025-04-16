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
        [HttpPatch("ShareLocation")]
        public IActionResult ShareLocation(LocationDto locationDto)
        {
            string? userGuid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userGuid is null)
            {
                return Unauthorized();
            }
            if (locationDto is not null)
            {
                bool response =  _userService.UpdateLocation(locationDto, userGuid);
                return Ok(response);
            }
            return NotFound();
           
           

        }


        [Authorize]
        [HttpPost("CloseConnection")]
        public async Task<ActionResult> CloseConnectionWithSocket()
        {
            string? userGuid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userGuid == null) return Unauthorized();
          
            string userClientGuid = _userUtility.GetClientGuid(userGuid);
            await _webSocketConnection.CloseConnection(userClientGuid);
            return Ok();
        }

        [HttpGet("GetFollowings")]
        public ICollection<ResponseFollowingDto>? GetFollowings(string clientGuid)
        {
            ICollection<ResponseFollowingDto> users = _userService.GetFollowings(clientGuid);
            if (users != null)
            {
                return users;
            }
            else return null;
        }
        [HttpGet("GetFollowers")]
        public ICollection<ResponseFollowersDto>? GetFollowers(string clientGuid)
        {
            ICollection<ResponseFollowersDto> users = _userService.GetFollowers(clientGuid);
            if (users != null)
            {
                return users;
            }
            else return null;
        }
        [Authorize]
        [HttpPost("Follow")]
        public async Task<IActionResult> Follow([FromQuery] string targetClientGuid)
        {
            string? userGuid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (targetClientGuid == null) return BadRequest();
            
            bool response = await _userService.Follow(targetClientGuid, userGuid);
            if (response)
            {
                return Ok();
            }
            else return NotFound();
        }
        [Authorize]
        [HttpGet("GetUserProfile")]
        public UserProfileDto? GetUserData(string clientGuid)
        {
            string? userGuid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            UserProfileDto user = _userService.GetUserProfileData(userGuid, clientGuid);

            if(user != null)
            {
                return user;
            }
            return null;
        }

        [HttpGet("IsFollowing")]
        public ActionResult<bool> IsFollowing([FromQuery] string targetClientGuid)
        {
            string? userGuid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            FollowResponse followResponse = _userService.IsFollowing(targetClientGuid, userGuid);

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
            
            string? userGuid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            string clientGuid = _userUtility.GetClientGuid(userGuid);
            if (userGuid == null) return Unauthorized();
            if (clientGuid != profileDto.ClientGuid) return Forbid();
           

            bool response = await this._userService.ChangePicture(profileDto, userGuid);
            if (response) return Ok(new { message = "Profile picture changed successfully" });
            return NotFound();
            
        }
        [Authorize]
        [HttpPut("ChangeUsername")]
        public ActionResult<bool> ChangeUsername([FromForm] ProfileDto profileDto)
        {
            string? userGuid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userGuid == null) return Unauthorized();

            bool response = _userService.ChangeUsername(userGuid, profileDto.Username);

            if (response) return Ok(new { message = "Username changed successfully"} );
            return NotFound(new { message = "Error!" });
            
        }
        [Authorize]
        [HttpGet("GetProfilePicture")]
        public ActionResult<string> GetProfilePicture(string clientGuid)
        {
           
            string? pictureUrl = _userService.GetPicture(clientGuid);
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

        [Authorize]
        [HttpGet("GetCurrentUserData")]
        public ActionResult<UserProfileDto> GetCurrentUserData()
        {
            string? userGuid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userGuid == null) return Unauthorized();

            UserProfileDto response = _userService.GetCurrentUserData(userGuid);
            return Ok(response);
        }

        
    } 
}
