using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PixelNestBackend.Dto;
using PixelNestBackend.Dto.Projections;
using PixelNestBackend.Interfaces;
using System.Security.Claims;

namespace PixelNestBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {

            _notificationService = notificationService;

        }
        [Authorize]
        [HttpPost("MarkAsOpened")]
        public ActionResult<bool> MarkAsOpened([FromBody]MarkAsOpenedDto markAsOpened)
        {
            string? userGuid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userGuid == null) return Unauthorized();

            bool result = _notificationService.MarkAsRead(markAsOpened, userGuid);
            return Ok(result);
        }
        [Authorize]
        [HttpGet("CountNotifications")]
        public ActionResult<int> CountNotifications()
        {
            string? userGuid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userGuid == null) return Unauthorized();

            int notifications = _notificationService.CountNotifications(userGuid);
            return Ok(notifications);
        }
        [Authorize]
        [HttpGet("GetAllNotifications")]
        public ActionResult<ICollection<ResponseNotificationsDto>> GetAllNotifications()
        {
            string? userGuid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userGuid == null) return Unauthorized();

            ICollection<ResponseNotificationsDto> result = _notificationService.GetNotifications(userGuid);
           
            if (result != null) return Ok(result);
            else return NotFound();

        }
    }
}
