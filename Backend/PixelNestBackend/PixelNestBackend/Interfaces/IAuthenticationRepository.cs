using Microsoft.AspNetCore.Mvc;
using PixelNestBackend.Dto;
using PixelNestBackend.Models;
using PixelNestBackend.Responses;

namespace PixelNestBackend.Interfaces
{
    public interface IAuthenticationRepository
    {
        bool Register(User registerDto);
        bool IsRegistered(User registerDto);
        LoginResponse Login(LoginDto loginDto);
    }
}
