using PixelNestBackend.Data;
using PixelNestBackend.Dto.Google;
using PixelNestBackend.Interfaces;
using PixelNestBackend.Models;
using PixelNestBackend.Responses.Google;

namespace PixelNestBackend.Repository
{
    public class GoogleRepository : IGoogleRepository
    {
        private readonly DataContext _dataContext;

        public GoogleRepository(DataContext dataContext) {
            _dataContext = dataContext;
        }
        public bool IsUserRegistered(string email)
        {
            bool isRegistered = _dataContext.Users.Where(e => e.Email == email).Any();
            return isRegistered;
        }

        public GoogleLoginResponse LoginWithGoogle(string email)
        {
            try
            {
                User user = _dataContext.Users.Where(u => u.Email.Equals(email)).FirstOrDefault();

                if (user == null)
                {
                    return null;
                }
                return new GoogleLoginResponse
                {
                    ClientGuid = user.ClientGuid,
                    UserGuid = user.UserGuid,
                    Email = email,
                    Username = user.Username,
                    IsSuccessful = true,
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        public GoogleAccountResponse RegisterGoogleAccount(User user)
        {
            try
            {
                _dataContext.Users.Add(user);
                _dataContext.SaveChanges();
                return new GoogleAccountResponse
                {
                    IsSuccessful = true,
                    Message = "Registered",
                    UserGuid = user.UserGuid,
                    ClientGuid = user.ClientGuid
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new GoogleAccountResponse
                {
                    IsSuccessful = false,
                    Message = "SQL Error"
                };
            }
        }
    }
}
