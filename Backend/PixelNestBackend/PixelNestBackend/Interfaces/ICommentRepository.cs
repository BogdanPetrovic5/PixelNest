namespace PixelNestBackend.Interfaces
{
    public interface ICommentRepository
    {
        bool LikeComment(int userID, int commentID);
    }
}
