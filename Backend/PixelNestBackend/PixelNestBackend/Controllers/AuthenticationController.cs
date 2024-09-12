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
        private readonly IUserService _userService;
        private readonly PasswordEncoder _passwordEncoder;
        public AuthenticationController(
                IConfiguration configuration, 
                IAuthenticationRepository authenticationRepository, 
                IUserService userService,
                PasswordEncoder passwordEncoder
            )
        {
            _configuration = configuration;
            _authenticationRepository = authenticationRepository;
            _userService = userService;
            _passwordEncoder = passwordEncoder;
        }

        [HttpPost("Register")]
        public IActionResult Register(RegisterDto registerDto)
        {
            if (registerDto == null)
            {
                return BadRequest();
            }
            User user = _userService.ConvertRegisterDto(registerDto);
            if (user != null) {
                user.Password = _passwordEncoder.EncodePassword(registerDto.Password);
                user.TotalLikes = 0;
                user.TotalPosts = 0;

                bool isEmailRegistered = _authenticationRepository.IsEmailRegistered(user);
                bool isUsernameRegistered = _authenticationRepository.IsUsernameRegistered(user);

             
                if (isEmailRegistered && isUsernameRegistered)
                {
                    return BadRequest(new { message = "A user with this email and username is already registered."});
                }
                else if (isEmailRegistered)
                {
                    return BadRequest(new { message = "A user with this email is already registered." });
                }
                else if (isUsernameRegistered)
                {
                    return BadRequest(new { message = "A user with this username is already registered." });
                }

                var result = _authenticationRepository.Register(user);
                if (result != null)
                {
                    return Ok(new { Message = "User registered successfully." });
                }
                else BadRequest("User registration failed.");
          
               
            }

            return BadRequest();
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
