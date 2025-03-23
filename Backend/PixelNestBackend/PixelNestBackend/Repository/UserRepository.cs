using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Caching.Memory;
using PixelNestBackend.Data;
using PixelNestBackend.Dto;
using PixelNestBackend.Dto.Projections;
using PixelNestBackend.Dto.WebSockets;
using PixelNestBackend.Gateaway;
using PixelNestBackend.Interfaces;
using PixelNestBackend.Models;
using PixelNestBackend.Responses;
using PixelNestBackend.Security;
using PixelNestBackend.Services.Menagers;
using PixelNestBackend.Utility;

namespace PixelNestBackend.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _dataContext;
        private readonly ILogger<UserRepository> _logger;
        private readonly UserUtility _userUtility;
        private readonly SASTokenGenerator _sasTokenGenerator;
        private readonly IMemoryCache _memoryCache;
        private const string UserCache = "User_{0}";
        public readonly IFileUpload _fileUpload;
        private readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(10);
        private readonly FolderGenerator _folderGenerator;
        private readonly BlobStorageUpload _blobStorageUpload;
        private readonly string _basedFolderPath;
        private readonly WebSocketConnectionMenager _websocketMenager;

        //private readonly IMemoryCache 
        public UserRepository(
            DataContext dataContext,
            ILogger<UserRepository> logger,
            UserUtility userUtility,
            SASTokenGenerator SASTokenGenerator,
            IMemoryCache memoryCache,
            FolderGenerator folderGenerator,
            BlobStorageUpload blobStorageUpload,
            IFileUpload fileUpload,
            WebSocketConnectionMenager webSocketConnectionMenager

            )
        {
            _websocketMenager = webSocketConnectionMenager;
            _dataContext = dataContext;
            _logger = logger;
            _userUtility = userUtility;
            _sasTokenGenerator = SASTokenGenerator;
            _memoryCache = memoryCache;
            _fileUpload = fileUpload;
            _blobStorageUpload = blobStorageUpload;
            _folderGenerator = folderGenerator;
            _basedFolderPath = Path.Combine("wwwroot", "Photos");
        }
        public FollowResponse IsFollowing(string targetClientGuid, string userGuid)
        {
            try
            {
                Guid targetUserGuid = _userUtility.GetUserID(targetClientGuid);
                Console.WriteLine("\nTarget user guid: " + targetUserGuid);
                Console.WriteLine("\nUser guid: " + userGuid);
                User followerUser = _dataContext.Users.Where(u => u.UserGuid.Equals(Guid.Parse(userGuid))).FirstOrDefault();
                User followingUser = _dataContext.Users.Where(u => u.UserGuid.Equals(targetUserGuid)).FirstOrDefault();

                if (followerUser != null && followingUser != null) {
                    bool isFollowing = _dataContext.Follow.Any(a => a.UserFollowerGuid == followerUser.UserGuid && a.UserFollowingGuid == followingUser.UserGuid);

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
        public ICollection<ResponseFollowingDto> GetFollowings(string clientGuid)
        {
            try
            {
                User user = _dataContext.Users.Where(u => u.ClientGuid.ToString().Equals(clientGuid)).FirstOrDefault();
                if (user != null)
                {
                    ICollection<ResponseFollowingDto> users = _dataContext
                    .Follow
                    .Where(follower => follower.UserFollowerGuid == user.UserGuid && follower.UserFollowingGuid != user.UserGuid)
                    .Select(f => new ResponseFollowingDto
                    {
                        FollowingUsername = f.UserFollowing.Username,
                        FollowingClientGuid = f.UserFollowing.ClientGuid

                    }).ToList();
                    return users;
                }return null;
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fatal error occurred: {ex.Message}");
                return null;
            }
        }
        public ICollection<ResponseFollowersDto> GetFollowers(string clientGuid)
        {
            try
            {
                User user = _dataContext.Users.Where(u => u.ClientGuid.ToString().Equals(clientGuid)).FirstOrDefault();
                ICollection<ResponseFollowersDto> users = _dataContext
                    .Follow
                    .Where(follower => follower.UserFollowingGuid == user.UserGuid && follower.UserFollowerGuid != user.UserGuid)
                    .Select(f => new ResponseFollowersDto
                    {
                        FollowerUsername = f.UserFollower.Username,
                        FollowerClientGuid = f.UserFollower.ClientGuid
                        

                    }).ToList();
                return users;
            }catch(Exception ex)
            {
                Console.WriteLine($"Fatal error occurred: {ex.Message}");
                return null;
            }
        }

        public FollowResponse Follow(string targetClientGuid, string userGuid)
        {
            try
            {
                bool isSaved = false;
                Follow follow = new Follow {

                
                    UserFollowerGuid = Guid.Parse(userGuid),
                    UserFollowingGuid = _userUtility.GetUserID(targetClientGuid)
                   
                };

                var isDuplicate = _dataContext
                    .Follow
                    .FirstOrDefault(uid => uid.UserFollowerGuid == follow.UserFollowerGuid && uid.UserFollowingGuid == follow.UserFollowingGuid);
                Notification notification = _dataContext.Notifications.Where(a=> a.SenderGuid == follow.UserFollowerGuid && a.ReceiverGuid == follow.UserFollowingGuid && a.PostID == null).FirstOrDefault();
              
              
                if(isDuplicate != null)
                {
                    _dataContext.Follow.Remove(isDuplicate);

                    User duplicateFollower = _dataContext.Users.FirstOrDefault(u => u.UserGuid == follow.UserFollowerGuid);
                    User duplicateFollowing = _dataContext.Users.FirstOrDefault(u => u.UserGuid == follow.UserFollowingGuid);
                    if(duplicateFollower != null && duplicateFollowing != null)
                    {
                        duplicateFollower.Following -= 1;
                        duplicateFollowing.Followers -= 1;
                    }
                    if(notification != null)
                    {
                        _dataContext.Notifications.Remove(notification);
                    }
                    isSaved =  _dataContext.SaveChanges() > 0;
                    if (isSaved)
                    {
                        return new FollowResponse
                        {
                            IsDuplicate = true,
                            IsSuccessful = true,
                            
                        };
                    }
                }


                notification = new Notification
                {
                    ReceiverGuid = follow.UserFollowingGuid,
                    SenderGuid = follow.UserFollowerGuid,
                    ParentCommentID = null,
                    PostID = null,
                    CommentID = null,
                    PostGuid = null,
                    DateTime = DateTime.UtcNow,
                    LikeID = null,
                    Message = "followed you."

                };
                
                _dataContext.Notifications.Add(notification);
              
                _dataContext.Follow.Add(follow);
                User followerUser = _dataContext.Users.FirstOrDefault(u => u.UserGuid == follow.UserFollowerGuid);
                User followingUser = _dataContext.Users.FirstOrDefault(u => u.UserGuid == follow.UserFollowingGuid);

                if (followerUser != null && followingUser != null)
                {
                    followerUser.Following += 1;
                    followingUser.Followers += 1;
                }
                isSaved = _dataContext.SaveChanges() > 0;
                if (isSaved)
                {
                    return new FollowResponse
                    {
                        IsDuplicate = false,
                        IsSuccessful = true,
                        TargetUser = _userUtility.GetClientGuid(follow.UserFollowingGuid.ToString()),
                        User = _userUtility.GetUserName(Guid.Parse(userGuid))

                    };
                }
                return new FollowResponse
                {
                    IsDuplicate = true,
                    IsSuccessful = false,

                };
            }
            catch(SqlException ex)
            {
                _logger.LogError(ex.Message);
                return new FollowResponse
                {
                    IsDuplicate = true,
                    IsSuccessful = false,

                };
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return new FollowResponse
                {
                    IsDuplicate = true,
                    IsSuccessful = false,

                };
            }
        }

        public UserProfileDto GetUserProfileData(string userGuid, string targetUser)
        {
            try
            {
                Guid clientGuid = Guid.Parse(_userUtility.GetClientGuid(userGuid));
                Guid targetGuid = Guid.Parse(targetUser);
                UserProfileDto? user = _dataContext
                    .Users
                    .Where(u => (u.ClientGuid).ToString() == targetUser)
                    .Select(up => new UserProfileDto
                    {
                        Username = up.Username,
                        Followers = up.Followers,
                        Followings = up.Following,
                        TotalPosts = up.TotalPosts,
                        Name = up.Firstname,
                        Lastname = up.Lastname,
                        ClientGuid = up.ClientGuid,
                        CanFollow = userGuid == (up.UserGuid).ToString() ? false : true,
                        CanEdit = userGuid == up.UserGuid.ToString() ? false : true,
                        ChatID = clientGuid.CompareTo(targetGuid) < 0
                         ? $"{clientGuid}-{targetGuid}"
                         : $"{targetGuid}-{clientGuid}"


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

        public string GetPicture(Guid userID)
        {
            try
            {
                var cacheKey = string.Format(UserCache, userID);
                var versionKey = $"{cacheKey}_Version";
                if (!_memoryCache.TryGetValue(versionKey, out DateTime cachedVersion))
                {
                   
                    cachedVersion = DateTime.MinValue;
                }
                else cachedVersion = DateTime.MaxValue;
                var latestVersion = DateTime.UtcNow;
                if (!_memoryCache.TryGetValue(cacheKey, out ImagePath image) || cachedVersion < latestVersion)
                {
                    
                    image = _dataContext.ImagePaths.Where(u => u.UserGuid == userID).FirstOrDefault();
                    _memoryCache.Set(cacheKey, image, new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = CacheDuration
                    });
                  
                }
                    
                if (image != null)
                {
                    //_sasTokenGenerator.appendSasToken(image);
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

        public ICollection<ResponseUsersDto> FindUsers(string username)
        {
            try
            {
                ICollection<ResponseUsersDto> responseUsersDto = _dataContext.Users
                    .Where(u => u.Username.Contains(username))
                    .Select(u => new ResponseUsersDto
                    {
                        Username = u.Username,
                        ClientGuid = u.ClientGuid
                        

                    }).ToList();
                return responseUsersDto;
            }
            catch (SqlException ex)
            {
                
                _logger.LogError(ex.Message);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
            
        }

        public async Task<bool> ChangeProfilePicture(string userGuid, ProfileDto profileDto)
        {
            Guid userID = Guid.Parse(userGuid);

            string userFolderName = userID.ToString();
            string userFolderPath = Path.Combine(_basedFolderPath, userFolderName, "Profile");


            if (!this._folderGenerator.CheckIfFolderExists(userFolderPath))
            {
                _folderGenerator.GenerateNewFolder(userFolderPath);
            }


            bool response = await _fileUpload.StoreImages(null, null, profileDto, userFolderPath, null, userID);
            //bool response = await _blobStorageUpload.StoreImages(null, null, profileDto, userID, null);
            var cacheKey = string.Format(UserCache, userID);
            var versionKey = $"{cacheKey}_Version";
            _memoryCache.Remove(versionKey);
            if (response) return true;
            return false;
        }

        public UserProfileDto GetCurrentUserData(string userGuid)
        {
            UserProfileDto userData = _dataContext.Users
                .Where(u => u.UserGuid.ToString() == userGuid)
                .Select(u => new UserProfileDto
                {
                    Name = u.Firstname,
                    Lastname = u.Lastname,
                    Email = u.Email,
                    ClientGuid = u.ClientGuid,
                    Username = $"{u.Firstname} {u.Lastname}",
                    ProfileImagePath = u.ProfilePhoto.Path
                }).FirstOrDefault();
            return userData;
        }

        public bool CheckIfUsernameExists(string username)
        {
            return _dataContext.Users.Where(u => u.Username.Equals(username)).Any();
        }
    }
}
