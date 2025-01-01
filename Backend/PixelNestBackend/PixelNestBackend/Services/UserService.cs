using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PixelNestBackend.Dto;
using PixelNestBackend.Dto.Projections;
using PixelNestBackend.Interfaces;
using PixelNestBackend.Mappers;
using PixelNestBackend.Models;
using PixelNestBackend.Repository;
using PixelNestBackend.Responses;
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
        public UserService(
            IMapper mapper,
            IAuthenticationRepository authenticationRepository,
            IUserRepository userRepository,
            IFileUpload fileUpload,
            UserUtility userUtility,
            FolderGenerator folderGenerator
            
            )
        {
            _userMapper = mapper;
            _authenticationRepository = authenticationRepository;
            _userRepository = userRepository;
            _userUtility = userUtility;
            _basedFolderPath = Path.Combine("wwwroot", "Photos");
            _folderGenerator = folderGenerator;
            _fileUpload = fileUpload;
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
        public bool Follow(FollowDto followDto)
        {
            return _userRepository.Follow(followDto);
        }

        public FollowResponse IsFollowing(FollowDto followDto)
        {
            return _userRepository.IsFollowing(followDto);
        }

        public async Task<bool> ChangePicture(ProfileDto profileDto, string email)
        {

            string? username = this._userUtility.GetUserName(email);
            int userID = this._userUtility.GetUserID(username);
            string userFolderName = userID.ToString();
            string userFolderPath = Path.Combine(_basedFolderPath, userFolderName, "Profile");


            if (!this._folderGenerator.CheckIfFolderExists(userFolderPath))
            {
                _folderGenerator.GenerateNewFolder(userFolderPath);
            }

            
            bool response = await _fileUpload.StoreImages(null, null, profileDto, userFolderPath, null, userID);
            if (response) return true;
            return false;

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
    }
}
