using PixelNestBackend.Dto;
using PixelNestBackend.Dto.Projections;
using PixelNestBackend.Models;
using PixelNestBackend.Responses;

namespace PixelNestBackend.Interfaces
{
    public interface IUserService
    {
        User ConvertRegisterDto(RegisterDto registerDto);
        bool IsEmailRegistered(User user);
        bool IsUsernameRegistered(User user);

        UserProfileDto GetUserProfileData(string username);
        bool Follow(FollowDto followDto);
        FollowResponse IsFollowing(FollowDto followDto);
        ICollection<ResponseFollowersDto> GetFollowers(string username);
        ICollection<ResponseFollowingDto> GetFollowings(string username);
        Task<bool> ChangePicture(ProfileDto profileDto, string email);
        string GetPicture(string username);

        bool ChangeUsername(string email, string newUsername);
        ICollection<ResponseUsersDto> FindUsers(string username);
        
    }
}
