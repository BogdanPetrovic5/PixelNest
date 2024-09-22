using PixelNestBackend.Data;

namespace PixelNestBackend.Utility
{
    public class CommentUtility
    {
        private readonly DataContext _dataContext;
        public CommentUtility(DataContext dataContext)
        {
            this._dataContext = dataContext;
        }

        public bool FindDuplicates(int userID, int commentID)
        {
            return _dataContext.LikeComments.Any(c => c.CommentID == commentID && c.UserID == userID);
        }
    }
}
