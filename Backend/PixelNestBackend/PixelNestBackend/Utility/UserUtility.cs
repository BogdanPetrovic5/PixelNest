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
            int userID = 0;
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
    }
}
