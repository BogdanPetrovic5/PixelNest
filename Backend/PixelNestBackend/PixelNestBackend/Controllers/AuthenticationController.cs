using CarWebShop.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PixelNestBackend.Dto;
using PixelNestBackend.Interfaces;
using PixelNestBackend.Models;
using PixelNestBackend.Services;

namespace PixelNestBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        public AuthenticationController(
                IAuthenticationService authenticationService
            )
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("Register")]
        public IActionResult Register(RegisterDto registerDto)
        {

            if (registerDto == null)
            {
                return BadRequest();
            }
            var response = _authenticationService.Register(registerDto);
            if (response == null)
            {
                return NotFound();
            }
            if (response.IsSuccess == false) 
                return NotFound(new { message = response.Message });
           
            return Ok(new { message = response.Message });
            
            
        }
        [HttpPost("Logout")]
        public IActionResult Logout([FromBody] LogoutDto logoutDto)
        {
            string token = _authenticationService.ReturnToken(logoutDto.Email);
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.Now.AddMinutes(-1)
            };
            Response.Cookies.Append("jwtToken", token, cookieOptions);
            return Ok();
        }

        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginDto loginDto)
        {
            var response = _authenticationService.Login(loginDto);
            if(response == null || response.IsSuccessful == false)
            {
                return NotFound();
            }
          
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.Now.AddMinutes(30)

            };

            Response.Cookies.Append("jwtToken", response.Token, cookieOptions);
            return Ok(new
            {
                Response = response.Response,
                IsSuccessful = response.IsSuccessful,
                Username = response.Username,
                Email = response.Email

            });
          

        }
        [Authorize]
        [HttpPost("IsLoggedIn")]
        public IActionResult IsLoggedIn()
        {
            return Ok(new {loggedIn = true});
        }

    }
}
