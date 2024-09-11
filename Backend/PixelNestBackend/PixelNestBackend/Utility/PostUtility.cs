using PixelNestBackend.Data;
using PixelNestBackend.Dto;

namespace PixelNestBackend.Utility
{
    public class PostUtility
    {
        private readonly IConfiguration _configuration;
        private readonly DataContext _dataContext;
        public PostUtility(IConfiguration configuration, DataContext dataContext) {
            _configuration = configuration;
            _dataContext = dataContext;
        }
        public bool FindDuplicate(int postID, int userID)
        {

            return _dataContext.LikedPosts.Any(
                lp => lp.PostID == postID && lp.UserID == userID);
        }
    }
}
