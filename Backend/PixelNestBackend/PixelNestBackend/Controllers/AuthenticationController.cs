using CarWebShop.Security;
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
        private readonly IConfiguration _configuration;
        private readonly IAuthenticationRepository _authenticationRepository;

        private readonly IAuthenticationService _authenticationService;
        private readonly IUserService _userService;
        private readonly PasswordEncoder _passwordEncoder;


        public AuthenticationController(
                IConfiguration configuration, 
                IAuthenticationRepository authenticationRepository, 
                IAuthenticationService authenticationService,
                IUserService userService,
                PasswordEncoder passwordEncoder
            )
        {
            _configuration = configuration;
            _authenticationRepository = authenticationRepository;
            _userService = userService;
            _passwordEncoder = passwordEncoder;
            _authenticationService = authenticationService;
        }

        [HttpPost("Register")]
        public IActionResult Register(RegisterDto registerDto)
        {

            if(registerDto == null)
            {
                return BadRequest();
            }
            var response = _authenticationService.Register(registerDto);
            if (response != null)
            {
                if(response.IsSuccess)
                {
                    return Ok(response);
                }
                return BadRequest(response);
            }
            return BadRequest(new { Message = "No response!" });
        }
        [HttpPost("Logout")]
        public IActionResult Logout([FromBody] LogoutDto logoutDto)
        {
            string token = _authenticationRepository.ReturnToken(logoutDto.Email);
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.Now.AddMinutes(-1)
            };
            Response.Cookies.Append("jwtToken", token, cookieOptions);
            return Ok();
        }

        [HttpPost("Login")]
        public IActionResult Login([FromBody]LoginDto loginDto)
        {
            var response = _authenticationRepository.Login(loginDto);
            if (response.IsSuccessful)
            {
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Expires = DateTime.Now.AddMinutes(30)
                    /*    SameSite = SameSiteMode.None,
       ,
                        Domain = "localhost",
                        Path = "/"*/
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
            return NotFound(new { Response = response.Response });
        }

    }
}
