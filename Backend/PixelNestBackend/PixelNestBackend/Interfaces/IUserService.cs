using PixelNestBackend.Dto;
using PixelNestBackend.Models;

namespace PixelNestBackend.Interfaces
{
    public interface IUserService
    {
        User ConvertRegisterDto(RegisterDto registerDto);
        bool IsEmailRegistered(User user);
        bool IsUsernameRegistered(User user);

        UserProfileDto GetUserProfileData(string username);
        bool Follow(FollowDto followDto);

    }
}
