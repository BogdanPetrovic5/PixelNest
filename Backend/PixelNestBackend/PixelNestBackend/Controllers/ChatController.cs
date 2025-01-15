using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PixelNestBackend.Dto;
using PixelNestBackend.Dto.Projections;
using PixelNestBackend.Interfaces;
using PixelNestBackend.Responses;
using PixelNestBackend.Services;
using PixelNestBackend.Services.Menagers;
using PixelNestBackend.Utility;
using System.Security.Claims;

namespace PixelNestBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;
        private readonly WebSocketConnectionMenager _websocketManager;
        private readonly UserUtility _userUtility;
        private readonly IUserService _userService;

        public ChatController(IChatService chatService, WebSocketConnectionMenager websocketManager, UserUtility userUtility, IUserService userService)
        {
            _chatService = chatService;
            _websocketManager = websocketManager;
            _userUtility = userUtility;
            _userService = userService;
        }

        [Authorize]
        [HttpGet("GetUserChats")]
        public ActionResult<ICollection<ResponseChatsDto>> GetUserChats() {

            string? email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (email == null) return Unauthorized();
            ICollection<ResponseChatsDto> chats = _chatService.GetUserChats(email);
            if (chats == null) return NotFound();
            return Ok(chats);
        }

        [Authorize]
        [HttpGet("GetUserToUserMessages")]
        public ActionResult<ICollection<ResponseMessagesDto>> GetUserToUserMessages(string targetUsername)
        {
            string? email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (email == null) return Unauthorized();
            string username = _userUtility.GetUserName(email);

            ICollection<ResponseMessagesDto> messages = _chatService.GetUserToUserMessages(username, targetUsername);
            return Ok(messages);
        }
        [Authorize]
        [HttpPost("SendMessage")]
        public async Task<ActionResult<bool>> SendMessage(MessageDto messageDto)
        {
            if (messageDto == null) return BadRequest();

            MessageResponse response = _chatService.SaveMessage(messageDto);

            if (response.IsSuccessfull)
            {
                 await _websocketManager.SendMessageToUser(messageDto.ReceiverUsername,messageDto.SenderUsername ,messageDto.Message);
                 return Ok(response.IsSuccessfull);
            }
            return BadRequest();
            
        }
        [Authorize]
        [HttpPost("LeaveRoom")]
        public ActionResult<bool> LeaveRoom([FromQuery] string receiverUsername)
        {
            string? email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (email == null) return Unauthorized();
            int userID = _userService.GetUserID(email);

            string username = _userUtility.GetUserName(email);
            string roomID = $"{username}-{receiverUsername}";
            string reverserdRoomID = $"{receiverUsername}-{username}";

            _websocketManager.LeaveRoom(roomID, reverserdRoomID, username);

            return Ok();
        }
        [Authorize]
        [HttpPost("JoinRoom")]
        public ActionResult<bool> JoinRoom([FromQuery] string receiverUsername)
        {
            string? email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (email == null) return Unauthorized();
            int userID = _userService.GetUserID(email);

            string username = _userUtility.GetUserName(email);
            string roomID = $"{username}-{receiverUsername}";
            string reverserdRoomID = $"{receiverUsername}-{username}";

            _websocketManager.JoinRoom(roomID, reverserdRoomID, username);

            return Ok();
        }
    }
}
