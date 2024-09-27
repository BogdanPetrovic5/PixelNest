using PixelNestBackend.Data;
using PixelNestBackend.Dto;
using PixelNestBackend.Interfaces;

namespace PixelNestBackend.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _dataContext;
        private readonly ILogger<UserRepository> _logger;
        public UserRepository(
            DataContext dataContext,
            ILogger<UserRepository> logger
            )
        {
            _dataContext = dataContext;
            _logger = logger;
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
                        Lastname = up.Lastname

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
