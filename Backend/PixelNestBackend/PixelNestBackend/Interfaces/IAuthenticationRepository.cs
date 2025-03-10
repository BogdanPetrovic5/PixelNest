using Microsoft.AspNetCore.Mvc;
using PixelNestBackend.Dto;
using PixelNestBackend.Models;
using PixelNestBackend.Responses;

namespace PixelNestBackend.Interfaces
{
    public interface IAuthenticationRepository
    {
        bool Register(User registerDto);
        bool IsEmailRegistered(User registerDto);
        bool IsUsernameRegistered(User registerDto);
        string ReturnToken(string userGuid);
        LoginResponse? Login(LoginDto loginDto);

    }
}
