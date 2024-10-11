using PixelNestBackend.Dto;
using PixelNestBackend.Responses;

namespace PixelNestBackend.Interfaces
{
    public interface IUserRepository
    {
        UserProfileDto GetUserProfileData(string username);
        bool Follow(FollowDto followDto);
        ICollection<ResponseFollowersDto> GetFollowers(string username);
        ICollection<ResponseFollowingDto> GetFollowings(string username);
        FollowResponse IsFollowing(FollowDto followDto);
    }
}
