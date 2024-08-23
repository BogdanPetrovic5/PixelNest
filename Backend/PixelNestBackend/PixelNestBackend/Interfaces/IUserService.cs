using PixelNestBackend.Dto;
using PixelNestBackend.Models;

namespace PixelNestBackend.Interfaces
{
    public interface IUserService
    {
        User ConvertRegisterDto(RegisterDto registerDto);
    }
}
