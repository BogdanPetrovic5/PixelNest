using CarWebShop.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PixelNestBackend.Dto;
using PixelNestBackend.Interfaces;
using PixelNestBackend.Models;
using PixelNestBackend.Responses;
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
        public ActionResult<RegisterResponse> Register(RegisterDto registerDto)
        {

            if (registerDto == null)
            {
                return BadRequest(new RegisterResponse { IsSuccess = false, Message = "Bad request" });
            }
            var response = _authenticationService.Register(registerDto);
            if (response == null)
            {
                return NotFound(new RegisterResponse { IsSuccess = false, Message = response.Message });
            }
            if (response.IsSuccess == false) 
                return NotFound(new RegisterResponse { IsSuccess = false, Message = response.Message});
           
            return Ok(new RegisterResponse { IsSuccess = true, Message = response.Message });
            
            
        }
        [HttpPost("Logout")]
        public IActionResult Logout([FromBody] LogoutDto logoutDto)
        {
            string token = _authenticationService.ReturnToken(logoutDto.Email);
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.None,
                Secure = true,
                Expires = DateTime.Now.AddMinutes(-1),
                Path = "/"
            };
            Response.Cookies.Append("jwtToken", token, cookieOptions);
            return Ok();
        }

        [HttpPost("Login")]
        public ActionResult<LoginResponse> Login([FromBody] LoginDto loginDto)
        {
            if(loginDto == null)
            {
                return BadRequest();
            }
            var response = _authenticationService.Login(loginDto);
            if (response == null)
            {
                return NotFound();
            }
            if(response != null && response.IsSuccessful == false)
            {
                return NotFound(new LoginResponse { 
                    IsSuccessful = response.IsSuccessful,
                    Response=response.Response
                });
            }
          
            var tokenCookieOption = new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.None,
                Secure = true,
                Expires = DateTime.Now.AddMinutes(30),
                Path = "/"

            };
            
            Response.Cookies.Append("jwtToken", response.Token, tokenCookieOption);
            return Ok(new LoginResponse
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
