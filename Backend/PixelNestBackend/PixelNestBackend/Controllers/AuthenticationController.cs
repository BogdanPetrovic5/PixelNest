using Azure;
using CarWebShop.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PixelNestBackend.Dto;
using PixelNestBackend.Dto.Google;
using PixelNestBackend.Interfaces;
using PixelNestBackend.Models;
using PixelNestBackend.Responses;
using PixelNestBackend.Responses.Google;
using PixelNestBackend.Services;
using PixelNestBackend.Utility;
using PixelNestBackend.Utility.Google;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace PixelNestBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly UserUtility _userUtility;
        private readonly IGoogleService _googleService;
        private readonly GoogleUtility _googleUtility;
        public AuthenticationController(
                IAuthenticationService authenticationService,
                UserUtility userUtility,
                IGoogleService googleService,
                GoogleUtility googleUtility
                
            )
        {
            _authenticationService = authenticationService;
            _userUtility = userUtility;
            _googleService = googleService;
            _googleUtility = googleUtility;
        }

        [HttpPost("SaveState")]
        public IActionResult SaveState([FromQuery] string state)
        {

            HttpContext.Session.SetString("oauth_state", state);
            return Ok();
        }

        [HttpGet("SigninGoogle")]
        public async Task<IActionResult> SigninGoogle([FromQuery] string code, [FromQuery] string state)
        {
            if (string.IsNullOrEmpty(code))
                return BadRequest("Authorization code is missing.");

            string sessionState = HttpContext.Session.GetString("oauth_state");
            if (state != sessionState)
            {
                return BadRequest("Invalid state");
            }

            GoogleTokenResponse googleToken = await _googleUtility.GetGoogleToken(code);
            if (googleToken == null)
            {
                return BadRequest("Failed to retrieve Google token");
            }

            
            GoogleAccountDto googleAccountDto = _googleUtility.GenerateGoogleAccountDto(googleToken.id_token);
            if (!_googleService.IsUserRegistered(googleAccountDto.Email))
            {
                GoogleAccountResponse googleAccountResponse = await _googleService.RegisterGoogleAccount(googleAccountDto);
            }

            GoogleLoginResponse loginResponse = await _googleService.LoginWithGoogle(googleAccountDto.Email);

            if (loginResponse != null && loginResponse.IsSuccessful)
            {
                var tokenExpirationDate = DateTime.Now.AddMinutes(30);
                loginResponse.TokenExpiration = tokenExpirationDate;
                HttpContext.Session.SetString(state, System.Text.Json.JsonSerializer.Serialize(loginResponse));
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    SameSite = SameSiteMode.None,
                    Secure = true,
                    Expires = tokenExpirationDate,
                    Path = "/"
                };
                Response.Cookies.Append("jwtToken", loginResponse.Token, cookieOptions);
                return Redirect("http://localhost:4200/Authentication/Redirect-Page");
            }


            return BadRequest();
            
        }
        [Authorize]
        [HttpGet("GetLoginResponse")]
        public IActionResult GetLoginResponse([FromQuery] string state)
        {
            if (string.IsNullOrEmpty(state))
            {
                return BadRequest("State is missing.");
            }

            string loginResponseJson = HttpContext.Session.GetString(state);
            if (string.IsNullOrEmpty(loginResponseJson))
            {
                return BadRequest("No login response found for the given state.");
            }

            return Ok(System.Text.Json.JsonSerializer.Deserialize<GoogleLoginResponse>(loginResponseJson));
        }


        [Authorize]
        [HttpPost("RefreshToken")]
        public ActionResult RefreshToken()
        {
            string? userGuid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            string token = _authenticationService.ReturnToken(userGuid);

            string username = _userUtility.GetUserName(userGuid);
            string email = _userUtility.GetEmail(userGuid);
            string clientGuid = _userUtility.GetClientGuid(userGuid);

            var tokenExpirationDate = DateTime.Now.AddMinutes(30);
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.None,
                Secure = true,
                Expires = tokenExpirationDate,
                Path = "/"
            };
            Response.Cookies.Append("jwtToken", token, cookieOptions);
            return Ok(new LoginResponse
            {
                Response = "Token refreshed!",
                IsSuccessful = true,
                Username = username,
                Email = email,
                TokenExpiration = tokenExpirationDate,
                ClientGuid = clientGuid
                

            });
           
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
            string? userGUid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            string token = _authenticationService.ReturnToken(userGUid);
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
            var tokenExpirationDate = DateTime.Now.AddMinutes(30);
            var tokenCookieOption = new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.None,
                Secure = true,
                Expires = tokenExpirationDate,
                Path = "/"

            };
            
            Response.Cookies.Append("jwtToken", response.Token, tokenCookieOption);
            return Ok(new LoginResponse
            {
                Response = response.Response,
                IsSuccessful = response.IsSuccessful,
                Username = response.Username,
                Email = response.Email,
                TokenExpiration = tokenExpirationDate,
                ClientGuid = response.ClientGuid

            });
          

        }
        [Authorize]
        [HttpGet("IsLoggedIn")]
        public IActionResult IsLoggedIn()
        {
            return Ok(new {loggedIn = true});
        }

    }
}
