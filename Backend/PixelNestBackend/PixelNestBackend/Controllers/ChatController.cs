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
    [Route("api/chat")]
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
        [HttpGet("unread-messages")]
        public ActionResult<int> GetNumberOfNewMessages()
        {
            string? userGuid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userGuid == null) return Unauthorized();
            int number = _chatService.GetNumberOfNewMessages(userGuid);
            return Ok(new {
                newMessages = number
            });
        }
        [Authorize]
        [HttpPost("message/read")]
        public ActionResult<bool> MarkAsRead([FromBody] MarkAsRead markAsReadDto)
        {
            string? userGuid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userGuid == null) return Unauthorized();
            bool response = _chatService.MarkAsRead(markAsReadDto, userGuid);
            return Ok(response);
        }


        [Authorize]
        [HttpGet("chats")]
        public ActionResult<ICollection<ResponseChatsDto>> GetUserChats() {

            string? userGuid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userGuid == null) return Unauthorized();
            ICollection<ResponseChatsDto> chats = _chatService.GetUserChats(userGuid);
            if (chats == null) return NotFound();
            return Ok(chats);
        }

        [Authorize]
        [HttpGet("messages")]
        public ActionResult<ICollection<ResponseMessagesDto>> GetUserToUserMessages(string chatID)
        {
            string? userGuid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userGuid == null) return Unauthorized();
           

            ICollection<ResponseMessagesDto> messages = _chatService.GetUserToUserMessages(chatID, userGuid);
            return Ok(messages);
        }
        [Authorize]
        [HttpPost("message/send")]
        public async Task<ActionResult<bool>> SendMessage(MessageDto messageDto)
        {
            string? userGuid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
           
            if (messageDto == null || userGuid == null) return BadRequest();

            MessageResponse response = _chatService.SaveMessage(messageDto, userGuid);

            if (response.IsSuccessfull)
            {
       

                await _websocketManager.SendMessageToUser(response);
                return Ok(response.IsSuccessfull);
            }
            return BadRequest();
            
        }
        [Authorize]
        [HttpPost("room/leave/{targetClientGuid}")]
        public ActionResult<bool> LeaveRoom(string targetClientGuid)
        {
            string? userGuid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userGuid == null) return Unauthorized();

            string currentClientGuid = _userUtility.GetClientGuid(userGuid);

            string roomID = $"{currentClientGuid}-{targetClientGuid}";
            string reverserdRoomID = $"{targetClientGuid}-{currentClientGuid}";

            _websocketManager.LeaveRoom(roomID, reverserdRoomID, currentClientGuid);

            return Ok(new
            {
           
            });
        }
        [Authorize]
        [HttpPost("room/join/{targetClientGuid}")]
        public ActionResult<bool> JoinRoom(string targetClientGuid)
        {
            string? userGuid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userGuid == null) return Unauthorized();

            string currentClientGuid = _userUtility.GetClientGuid(userGuid);
            
            string roomID = $"{currentClientGuid}-{targetClientGuid}";
            string reverserdRoomID = $"{targetClientGuid}-{currentClientGuid}";

            _websocketManager.JoinRoom(roomID, reverserdRoomID, currentClientGuid);

            return Ok(new
            {
                roomID = roomID,
                reverserdRoomID = reverserdRoomID
            });
        }

        [Authorize]
        [HttpGet("search")]
        public ActionResult<ICollection<ResponseChatsDto>> FindChats([FromQuery] string searchParameter)
        {
            string userGuid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userGuid == null || string.Empty == userGuid)
            {
                return Unauthorized();
            }
            ICollection<ResponseChatsDto> searchChats = _chatService.SearchChats(searchParameter, userGuid);
            return Ok(searchChats);
        }

    }
}
