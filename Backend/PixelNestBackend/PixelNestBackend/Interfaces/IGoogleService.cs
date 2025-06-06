using PixelNestBackend.Dto.Google;
using PixelNestBackend.Responses.Google;

namespace PixelNestBackend.Interfaces
{
    public interface IGoogleService
    {
        bool IsUserRegistered(string email);
        Task<GoogleAccountResponse> RegisterGoogleAccount(GoogleAccountDto googleAccountDto);
        Task<GoogleLoginResponse> LoginWithGoogle(GoogleAccountDto googleAccountDto);
        
    }
}
