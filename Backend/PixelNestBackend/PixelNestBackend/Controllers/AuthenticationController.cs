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
                bool isRegistered = _authenticationRepository.IsRegistered(user);
                if (!isRegistered)
                {
                    var result = _authenticationRepository.Register(user);
                    if (result != null)
                    {
                        return Ok(new { Message = "User registered successfully." });
                    }
                    else BadRequest("User registration failed.");
                }
                else return BadRequest("User with this email is already registered");
               
            }

            return BadRequest();
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
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.Now.AddHours(1)
                };
                Response.Cookies.Append("jwtToken", response.Token, cookieOptions);
                return Ok(new
                {
                    Response = response.Response,
                    IsSuccessful = response.IsSuccessful,
                    Username = response.Username
                    
                });

            }
            return NotFound(new { Response = response.Response });
        }

    }
}
