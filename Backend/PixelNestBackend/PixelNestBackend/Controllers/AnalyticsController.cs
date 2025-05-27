using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PixelNestBackend.Dto.Projections;
using PixelNestBackend.Interfaces;
using System.Security.Claims;

namespace PixelNestBackend.Controllers
{
    [Route("api/analytics")]
    [ApiController]
    public class AnalyticsController : ControllerBase
    {
        private readonly IAnalyticsService _analyticsService;
        public AnalyticsController(IAnalyticsService analyticsService) {
            _analyticsService = analyticsService;
        }
        [Authorize]
        [HttpGet("locations")]
        public ActionResult<ICollection<ResponseAnalyticsLocation>> AnalyticsLocation()
        {
            string? userGuid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userGuid == null) return Unauthorized();

            ICollection<ResponseAnalyticsLocation> response = _analyticsService.GetAnalyticsLocations(userGuid);


            
            return Ok(response);
        }
    }
}
