using Microsoft.Data.SqlClient;
using PixelNestBackend.Data;
using PixelNestBackend.Dto;
using PixelNestBackend.Dto.Projections;
using PixelNestBackend.Interfaces;
using PixelNestBackend.Models;
using PixelNestBackend.Responses;
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
        public FollowResponse IsFollowing(FollowDto follow)
        {
            try
            {
                User followerUser = _dataContext.Users.Where(u => u.Username.Equals(follow.FollowerUsername)).FirstOrDefault();
                User followingUser = _dataContext.Users.Where(u => u.Username.Equals(follow.FollowingUsername)).FirstOrDefault();

                if (followerUser != null && followingUser != null) {
                    bool isFollowing = _dataContext.Follow.Any(a => a.UserFollowerID == followerUser.UserID && a.UserFollowingID == followingUser.UserID);

                    return new FollowResponse
                    {
                        IsFollowing = isFollowing,
                        IsSuccessful = true
                    };
                }
                return new FollowResponse
                {
                    IsFollowing = false,
                    IsSuccessful = true
                };


            }
            catch(SqlException ex)
            {
                Console.WriteLine(ex.Message);
                return new FollowResponse
                {
                    IsFollowing = false,
                    IsSuccessful = false
                }; 
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new FollowResponse
                {
                    IsFollowing = false,
                    IsSuccessful = false
                }; 
            }
        }
        public ICollection<ResponseFollowingDto> GetFollowings(string username)
        {
            try
            {
                User user = _dataContext.Users.Where(u => u.Username.Equals(username)).FirstOrDefault();
                if (user != null)
                {
                    ICollection<ResponseFollowingDto> users = _dataContext
                    .Follow
                    .Where(follower => follower.UserFollowerID == user.UserID && follower.UserFollowingID != user.UserID)
                    .Select(f => new ResponseFollowingDto
                    {
                        FollowingUsername = f.UserFollowing.Username

                    }).ToList();
                    return users;
                }return null;
                
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
                User user = _dataContext.Users.Where(u => u.Username.Equals(username)).FirstOrDefault();
                ICollection<ResponseFollowersDto> users = _dataContext
                    .Follow
                    .Where(follower => follower.UserFollowingID == user.UserID && follower.UserFollowerID != user.UserID)
                    .Select(f => new ResponseFollowersDto
                    {
                        FollowerUsername = f.UserFollower.Username

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

        public string GetUsername(string email)
        {
            try
            {
                var user = _dataContext.Users.Where(e => e.Email == email).FirstOrDefault();

                if(user == null)
                {
                    return string.Empty;
                }
                return user.Username;
            }catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return string.Empty;
            }

        }
        public string GetPicture(int userID)
        {
            try
            {
                var image = _dataContext.ImagePaths.Where(u => u.UserID == userID).FirstOrDefault();
                if (image != null)
                {
                    return image.Path;
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return string.Empty;
            }
        }

        public bool ChangeUsername(string username, string newUsername)
        {
            try
            {
                var user = _dataContext.Users.Where(u => u.Username == username).FirstOrDefault();
                if (user != null) {
                    user.Username = newUsername;
                    return true;
                }
                return false;

            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
                return false;
                
            }
        }
    }
}
