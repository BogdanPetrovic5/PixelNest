using Microsoft.AspNetCore.Mvc;
using PixelNestBackend.Dto;
using PixelNestBackend.Models;

namespace PixelNestBackend.Interfaces
{
    public interface IAuthenticationRepository
    {
        bool Register(User registerDto);
    }
}
