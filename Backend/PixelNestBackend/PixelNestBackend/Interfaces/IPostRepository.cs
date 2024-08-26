using Microsoft.AspNetCore.Mvc;

namespace PixelNestBackend.Interfaces
{
    public interface IPostRepository
    {
        bool ShareNewPost(string email);
    }
}
