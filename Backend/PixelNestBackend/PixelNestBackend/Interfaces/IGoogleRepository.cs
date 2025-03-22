using PixelNestBackend.Dto.Google;
using PixelNestBackend.Models;
using PixelNestBackend.Responses.Google;

namespace PixelNestBackend.Interfaces
{
    public interface IGoogleRepository
    {
        bool IsUserRegistered(string email);
        GoogleAccountResponse RegisterGoogleAccount(User user);
        GoogleLoginResponse LoginWithGoogle(string email);
    }
}
