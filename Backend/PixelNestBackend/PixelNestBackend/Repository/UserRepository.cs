using Microsoft.Data.SqlClient;
using PixelNestBackend.Data;
using PixelNestBackend.Dto;
using PixelNestBackend.Interfaces;
using PixelNestBackend.Models;
using PixelNestBackend.Utility;

namespace PixelNestBackend.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _dataContext;
        private readonly ILogger<UserRepository> _logger;
        private readonly UserUtility _userUtility;
        public UserRepository(
            DataContext dataContext,
            ILogger<UserRepository> logger,
            UserUtility userUtility
            )
        {
            _dataContext = dataContext;
            _logger = logger;
            _userUtility = userUtility;
        }
        public ICollection<ResponseFollowingDto> GetFollowings(string username)
        {
            try
            {
                ICollection<ResponseFollowingDto> users = _dataContext
                    .Follow
                    .Where(follower => follower.FollowerUsername == username && follower.FollowingUsername != username)
                    .Select(f => new ResponseFollowingDto
                    {
                        FollowingUsername = f.FollowingUsername

                    }).ToList();
                return users;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Doslo je do fatalne greske: {ex.Message}");
                return null;
            }
        }
        public ICollection<ResponseFollowersDto> GetFollowers(string username)
        {
            try
            {
                ICollection<ResponseFollowersDto> users = _dataContext
                    .Follow
                    .Where(follower => follower.FollowingUsername == username && follower.FollowerUsername != username)
                    .Select(f => new ResponseFollowersDto
                    {
                        FollowerUsername = f.FollowerUsername

                    }).ToList();
                return users;
            }catch(Exception ex)
            {
                Console.WriteLine($"Doslo je do fatalne greske: {ex.Message}");
                return null;
            }
        }

        public bool Follow(FollowDto followDto)
        {
            try
            {
                Follow follow = new Follow {

                    FollowerUsername = followDto.FollowerUsername,
                    FollowingUsername = followDto.FollowingUsername,
                    UserFollowerID = _userUtility.GetUserID(followDto.FollowerUsername),
                    UserFollowingID = _userUtility.GetUserID(followDto.FollowingUsername)
                   
                };

                var isDuplicate = _dataContext
                    .Follow
                    .FirstOrDefault(uid => uid.UserFollowerID == follow.UserFollowerID && uid.UserFollowingID == follow.UserFollowingID);
                if(isDuplicate != null)
                {
                    _dataContext.Follow.Remove(isDuplicate);

                    User duplicateFollower = _dataContext.Users.FirstOrDefault(u => u.Username == followDto.FollowerUsername);
                    User duplicateFollowing = _dataContext.Users.FirstOrDefault(u => u.Username == followDto.FollowingUsername);
                    if(duplicateFollower != null && duplicateFollowing != null)
                    {
                        duplicateFollower.Following -= 1;
                        duplicateFollowing.Followers -= 1;
                    }

                    return _dataContext.SaveChanges() > 0;
                }


                _dataContext.Follow.Add(follow);
                User followerUser = _dataContext.Users.FirstOrDefault(u => u.Username == followDto.FollowerUsername);
                User followingUser = _dataContext.Users.FirstOrDefault(u => u.Username == followDto.FollowingUsername);
                
                if(followerUser != null && followingUser != null)
                {
                    followerUser.Following += 1;
                    followingUser.Followers += 1;
                }
                return _dataContext.SaveChanges() > 0;
            }catch(SqlException ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
        }

        public UserProfileDto GetUserProfileData(string username)
        {
            try
            {
                UserProfileDto user = _dataContext
                    .Users
                    .Where(u => u.Username == username)
                    .Select(up => new UserProfileDto
                    {
                        Username = up.Username,
                        Followers = up.Followers,
                        Followings = up.Following,
                        TotalPosts = up.TotalPosts,
                        Name = up.Firstname,
                        Lastname = up.Lastname,
                    

                    })
                    .FirstOrDefault();
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }
    }
}
