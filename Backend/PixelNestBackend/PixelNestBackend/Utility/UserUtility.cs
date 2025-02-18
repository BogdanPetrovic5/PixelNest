using Microsoft.Data.SqlClient;
using PixelNestBackend.Data;

namespace PixelNestBackend.Utility
{
    public class UserUtility { 


        private readonly DataContext _dataContext;
        public UserUtility(DataContext dataContext)
        {
             _dataContext = dataContext;
        }
        public Guid GetUserID(string username)
        {
            Guid userID = Guid.Empty;
            if (_dataContext != null)
            {
                var user =_dataContext.Users.FirstOrDefault(x => x.Username == username);
                if(user != null)
                {
                    userID = user.UserGuid;
                }

            }
            Console.WriteLine("\nUser GUID: " + userID + "\n");
            return userID;
        }
        public string GetUserName(Guid userID)
        {
            
            if (_dataContext != null)
            {
                var user = _dataContext.Users.FirstOrDefault(x => x.UserGuid == userID);
                if (user != null)
                {
                    return user.Username;
                }
                

            }
            return null;

        }
        public string GetUserName(string email)
        {

            if (_dataContext != null)
            {
                var user = _dataContext.Users.FirstOrDefault(x => x.Email == email);
                if (user != null)
                {
                    return user.Username;
                }


            }
            return null;

        }
    }
}
