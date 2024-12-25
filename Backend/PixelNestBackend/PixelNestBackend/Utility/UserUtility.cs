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
        public int GetUserID(string username)
        {
            int userID = -1;
            if (_dataContext != null)
            {
                var user =_dataContext.Users.FirstOrDefault(x => x.Username == username);
                if(user != null)
                {
                    userID = user.UserID;
                }

            }
            
            return userID;
        }
        public string GetUserName(int userID)
        {
            
            if (_dataContext != null)
            {
                var user = _dataContext.Users.FirstOrDefault(x => x.UserID == userID);
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
