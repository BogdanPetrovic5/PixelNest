using AutoMapper;
using PixelNestBackend.Dto;
using PixelNestBackend.Dto.Projections;
using PixelNestBackend.Interfaces;
using PixelNestBackend.Mappers;
using PixelNestBackend.Models;
using PixelNestBackend.Repository;
using PixelNestBackend.Responses;

namespace PixelNestBackend.Services
{

    public class UserService : IUserService
    {
        readonly private IMapper _userMapper;
        readonly private IAuthenticationRepository _authenticationRepository;
        readonly private IUserRepository _userRepository;

        public UserService(
            IMapper mapper,
            IAuthenticationRepository authenticationRepository,
            IUserRepository userRepository
            )
        {
            _userMapper = mapper;
            _authenticationRepository = authenticationRepository;
            _userRepository = userRepository;
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
    }
}
