﻿using PixelNestBackend.Dto;
using PixelNestBackend.Dto.Projections;
using PixelNestBackend.Responses;

namespace PixelNestBackend.Interfaces
{
    public interface IUserRepository
    {
        UserProfileDto GetUserProfileData(string username);
        FollowResponse Follow(FollowDto followDto);
        ICollection<ResponseFollowersDto> GetFollowers(string username);
        ICollection<ResponseFollowingDto> GetFollowings(string username);
        FollowResponse IsFollowing(FollowDto followDto);
        string GetUsername(string email);

        string GetPicture(Guid userID);
        bool ChangeUsername(string username, string newUsername);
        ICollection<ResponseUsersDto> FindUsers(string username);
        Task<bool> ChangeProfilePicture(Guid userID, ProfileDto profileDto);
        
    }
}
