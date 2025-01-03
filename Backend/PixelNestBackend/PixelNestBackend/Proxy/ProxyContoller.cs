using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PixelNestBackend.Proxy
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProxyContoller : ControllerBase
    {
        
        [HttpGet("GetLocation")]
        public IActionResult GetLocation()
        {
            return Ok();
        }
    }
}
