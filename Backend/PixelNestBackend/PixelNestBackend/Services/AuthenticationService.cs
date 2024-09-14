
using CarWebShop.Security;
using PixelNestBackend.Dto;
using PixelNestBackend.Interfaces;
using PixelNestBackend.Models;
using PixelNestBackend.Responses;
using System.Security.Claims;

namespace PixelNestBackend.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IAuthenticationRepository _authenticationRepository;
        private readonly IUserService _userService;
        private readonly PasswordEncoder _passwordEncoder;
        public AuthenticationService(
            IAuthenticationRepository authenticationRepository,
            IUserService userService,
            PasswordEncoder passwordEncoder
            ) {
            _authenticationRepository = authenticationRepository;
            _userService = userService;
            _passwordEncoder = passwordEncoder;
        }

        public LoginResponse Login(LoginDto loginDto)
        {
            throw new NotImplementedException();
        }

        public RegisterResponse Register(RegisterDto registerDto)
        {
            User user = _userService.ConvertRegisterDto(registerDto);
            if(user == null)
            {
                return new RegisterResponse
                {
                    IsSuccess = false,
                    Message = "User is null!"

                };
            }
            user.Password = _passwordEncoder.EncodePassword(registerDto.Password);
            user.TotalLikes = 0;
            user.TotalPosts = 0;
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
                return new RegisterResponse { Message = "A user with this username is already registered.", IsSuccess = false } ;
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
            throw new NotImplementedException();
        }
    }
}
