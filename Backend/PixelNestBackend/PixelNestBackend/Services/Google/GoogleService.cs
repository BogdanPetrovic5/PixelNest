using PixelNestBackend.Dto.Google;
using PixelNestBackend.Interfaces;
using PixelNestBackend.Models;
using PixelNestBackend.Responses.Google;
using PixelNestBackend.Security;
using PixelNestBackend.Utility;
using System.Text.Json;

namespace PixelNestBackend.Services.Google
{
    public class GoogleService : IGoogleService
    {
        private readonly IGoogleRepository _googleRepository;
        private readonly IFileUpload _fileUpload;
        private readonly TokenGenerator _tokenGenerator;
        private readonly UserUtility _userUtility;

        public GoogleService(IGoogleRepository googleRepository, IFileUpload fileUpload, TokenGenerator tokenGenerator, UserUtility userUtility)
        {
            _googleRepository = googleRepository;
            _fileUpload = fileUpload;
            _tokenGenerator = tokenGenerator;
            _userUtility = userUtility;
    }
        public bool IsUserRegistered(string email)
        {
            return _googleRepository.IsUserRegistered(email);
        }

        public async Task<GoogleLoginResponse> LoginWithGoogle(string email)
        {
            GoogleLoginResponse response = _googleRepository.LoginWithGoogle(email);

            response.Token = _tokenGenerator.GenerateToken(response.UserGuid.ToString());
            response.UserGuid = null;

            return response;
        }

        public async Task<GoogleAccountResponse> RegisterGoogleAccount(GoogleAccountDto googleAccountDto)
        {
            string randomUsername = _userUtility.GenerateRandomUsername();
            User user = new User
            {
                Email = googleAccountDto.Email,
                Firstname = googleAccountDto.Firstname,
                Lastname = googleAccountDto.Lastname,
                Username = randomUsername,
                TotalLikes = 0,
                TotalPosts = 0,
                Followers = 0,
                Following = 0,
            };
            GoogleAccountResponse googleAccountResponse = _googleRepository.RegisterGoogleAccount(user);
            await _fileUpload.StoreImages(null, null, null, googleAccountDto.Picture, null, googleAccountResponse.UserGuid);
            return googleAccountResponse;
        }
    }
}
