using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PixelNestBackend.Proxy
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProxyContoller : ControllerBase
    {
        private readonly string _mapTilerApiKey = "aqR39NWYQyZAdFc6KtYh";
        [HttpGet("GetLocation")]
        public IActionResult GetLocation()
        {
            return Ok();
        }
    }
}
