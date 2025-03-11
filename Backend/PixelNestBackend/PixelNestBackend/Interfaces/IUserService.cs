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

        UserProfileDto GetUserProfileData(string clientGuid, string targetUser);
        Task<bool> Follow(string targetClientGuid, string userGuid);
        FollowResponse IsFollowing(string targetClientGuid, string userGuid);
        ICollection<ResponseFollowersDto> GetFollowers(string clientGuid);
        ICollection<ResponseFollowingDto> GetFollowings(string clientGuid);
        Task<bool> ChangePicture(ProfileDto profileDto, string userGuid);
        string GetPicture(string username);

        bool ChangeUsername(string email, string newUsername);
        ICollection<ResponseUsersDto> FindUsers(string username);
        Guid GetUserID(string email);
    }
}
