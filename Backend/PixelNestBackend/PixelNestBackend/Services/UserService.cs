using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PixelNestBackend.Dto;
using PixelNestBackend.Dto.Projections;
using PixelNestBackend.Dto.WebSockets;
using PixelNestBackend.Gateaway;
using PixelNestBackend.Interfaces;
using PixelNestBackend.Mappers;
using PixelNestBackend.Models;
using PixelNestBackend.Repository;
using PixelNestBackend.Responses;
using PixelNestBackend.Services.Menagers;
using PixelNestBackend.Utility;

namespace PixelNestBackend.Services
{

    public class UserService : IUserService
    {
        readonly private IMapper _userMapper;
        readonly private IAuthenticationRepository _authenticationRepository;
        readonly private IUserRepository _userRepository;
        readonly private UserUtility _userUtility;
        public readonly IFileUpload _fileUpload;
        private readonly string _basedFolderPath;
        private readonly FolderGenerator _folderGenerator;
        private readonly BlobStorageUpload _blobStorageUpload;
        private readonly WebSocketConnectionMenager _webSocketConnectionMenager;
        public UserService(
            IMapper mapper,
            IAuthenticationRepository authenticationRepository,
            IUserRepository userRepository,
            IFileUpload fileUpload,
            UserUtility userUtility,
            FolderGenerator folderGenerator,
            BlobStorageUpload blobStorageUpload,
            WebSocketConnectionMenager webSocketConnectionMenager

            )
        {
            _webSocketConnectionMenager = webSocketConnectionMenager;
            _userMapper = mapper;
            _authenticationRepository = authenticationRepository;
            _userRepository = userRepository;
            _userUtility = userUtility;
            _basedFolderPath = Path.Combine("wwwroot", "Photos");
            _folderGenerator = folderGenerator;
            _fileUpload = fileUpload;
            _blobStorageUpload = blobStorageUpload;
        }
        public User ConvertRegisterDto(RegisterDto registerDto)
        {
            User user = _userMapper.Map<User>(registerDto);
            return user;
        }

        public UserProfileDto GetUserProfileData(string username)
        {
            UserProfileDto user = _userRepository.GetUserProfileData(username);
            if (user != null)
            {
                return user;
            }
            return null;
        }
        public int GetUserID(string email)
        {
            return _userUtility.GetUserID(email);
        }
        public bool IsEmailRegistered(User user)
        {
            return _authenticationRepository.IsEmailRegistered(user);
        }

        public bool IsUsernameRegistered(User user)
        {
            return _authenticationRepository.IsUsernameRegistered(user);
        }
        public ICollection<ResponseFollowersDto> GetFollowers(string username)
        {
            return _userRepository.GetFollowers(username);
        }
        public ICollection<ResponseFollowingDto> GetFollowings(string username)
        {
            return _userRepository.GetFollowings(username);
        }
        public async Task<bool> Follow(FollowDto followDto)
        {

            FollowResponse? response =  _userRepository.Follow(followDto);
            if (!response.IsDuplicate)
            {
                WebSocketMessage message = new WebSocketMessage
                {
                    SenderUsername = followDto.FollowerUsername,
                    TargetUser = followDto.FollowingUsername,
                    Type = "Follow",
                    Content = "followed you"
                };
                await _webSocketConnectionMenager.SendNotificationToUser(message);
            }
            return response.IsSuccessful;
        }

        public FollowResponse IsFollowing(FollowDto followDto)
        {
            return _userRepository.IsFollowing(followDto);
        }

        public async Task<bool> ChangePicture(ProfileDto profileDto, string email)
        {

            string? username = this._userUtility.GetUserName(email);
            int userID = this._userUtility.GetUserID(username);

            return await _userRepository.ChangeProfilePicture(userID, profileDto);

        }
        
        public string GetPicture(string username)
        {
            int userID = this._userUtility.GetUserID(username);

            
            return _userRepository.GetPicture(userID);
        }

        public bool ChangeUsername(string email, string newUsername)
        {
            string username = _userUtility.GetUserName(email);
            return _userRepository.ChangeUsername(username, newUsername);
        }

        public ICollection<ResponseUsersDto> FindUsers(string username)
        {
            return _userRepository.FindUsers(username);
        }
    }
}
