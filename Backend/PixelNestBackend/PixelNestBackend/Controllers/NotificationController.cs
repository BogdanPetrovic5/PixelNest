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
        public ActionResult<bool> MarkAsOpened([FromBody]MarkAsOpenedDto markAsRead)
        {
            string? email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (email == null) return Unauthorized();

            bool result = _notificationService.MarkAsRead(markAsRead, email);
            return Ok(result);
        }
        [Authorize]
        [HttpGet("CountNotifications")]
        public ActionResult<int> CountNotifications()
        {
            string? email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (email == null) return Unauthorized();

            int notifications = _notificationService.CountNotifications(email);
            return Ok(notifications);
        }
        [Authorize]
        [HttpGet("GetAllNotifications")]
        public ActionResult<ICollection<ResponseNotificationsDto>> GetAllNotifications()
        {
            string? email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (email == null) return Unauthorized();

            ICollection<ResponseNotificationsDto> result = _notificationService.GetNotifications(email);
           
            if (result != null) return Ok(result);
            else return NotFound();

        }
    }
}
