using PixelNestBackend.Dto;
using PixelNestBackend.Dto.Projections;
using PixelNestBackend.Responses;

namespace PixelNestBackend.Interfaces
{
    public interface IUserRepository
    {
        UserProfileDto GetUserProfileData(string username, string targetUser);
        FollowResponse Follow(string targetClientGuid, string userGuid);
        ICollection<ResponseFollowersDto> GetFollowers(string username);
        ICollection<ResponseFollowingDto> GetFollowings(string username);
        FollowResponse IsFollowing(string targetClientGuid, string userGuid);
   

        string GetPicture(Guid userID);
        bool ChangeUsername(string username, string newUsername);
        ICollection<ResponseUsersDto> FindUsers(string username);
        Task<bool> ChangeProfilePicture(string userGuid, ProfileDto profileDto);
        UserProfileDto GetCurrentUserData(string userGuid);
        bool CheckIfUsernameExists(string username);
        bool UpdateLocation(LocationDto locationDto, string userGuid);
    }
}
