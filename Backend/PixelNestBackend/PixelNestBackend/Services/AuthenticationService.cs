
using CarWebShop.Security;
using PixelNestBackend.Dto;
using PixelNestBackend.Interfaces;
using PixelNestBackend.Models;
using PixelNestBackend.Responses;
using PixelNestBackend.Security;
using System.Security.Claims;

namespace PixelNestBackend.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IAuthenticationRepository _authenticationRepository;
        private readonly IUserService _userService;
        private readonly PasswordEncoder _passwordEncoder;
        private readonly TokenGenerator _tokenGenerator;
        public AuthenticationService(
            IAuthenticationRepository authenticationRepository,
            IUserService userService,
            PasswordEncoder passwordEncoder,
             TokenGenerator tokenGenerator
            ) {
            _authenticationRepository = authenticationRepository;
            _userService = userService;
            _passwordEncoder = passwordEncoder;
            _tokenGenerator = tokenGenerator;
        }

        public LoginResponse Login(LoginDto loginDto)
        {
            var response = _authenticationRepository.Login(loginDto);
            if (response != null)
            {
                if (response.IsSuccessful)
                {
                    string email = response.Email;
                    string token = _tokenGenerator.GenerateToken(email);
                    return new LoginResponse
                    {
                        IsSuccessful = true,
                        Email = response.Email,
                        Username = response.Username,
                        Response = response.Response,
                        Token = token

                    };
                }
                return new LoginResponse { 
                    IsSuccessful = response.IsSuccessful,
                    Response = response.Response,
                };
            }
            return new LoginResponse
            {
                IsSuccessful = false,
                Response = "Login failed."
            };
        }

        public RegisterResponse? Register(RegisterDto registerDto)
        {
            User user = _userService.ConvertRegisterDto(registerDto);
            if(user == null)
            {
         
                return null;
            }
            user.Password = _passwordEncoder.EncodePassword(registerDto.Password);
            user.TotalLikes = 0;
            user.TotalPosts = 0;

            var validateUser = _validateUser(user);
          
            if (validateUser == null) return null;
 

            if (!validateUser.IsSuccess)
            {
                return validateUser;
            }
            bool result = _authenticationRepository.Register(user);
           
            if(result)
            {
                return new RegisterResponse
                {
                    IsSuccess = true,
                    Message = "User successfully registered!"
                };
            }
            return new RegisterResponse
            {
                IsSuccess = false,
                Message = "Registration failed!"
            };
        }

        public string ReturnToken(string email)
        {
            return _authenticationRepository.ReturnToken(email);
        }

        private RegisterResponse? _validateUser(User user)
        {
            bool isEmailRegistered = _authenticationRepository.IsEmailRegistered(user);
            bool isUsernameRegistered = _authenticationRepository.IsUsernameRegistered(user);


            if (isEmailRegistered && isUsernameRegistered)
            {
                return new RegisterResponse { Message = "A user with this email and username is already registered.", IsSuccess = false };
            }
            else if (isEmailRegistered)
            {
                return new RegisterResponse { Message = "A user with this email is already registered.", IsSuccess = false };
            }
            else if (isUsernameRegistered)
            {
                return new RegisterResponse { Message = "A user with this username is already registered.", IsSuccess = false };
            }
            return new RegisterResponse { Message = "User is valid!", IsSuccess = true };
        }
    }
}
