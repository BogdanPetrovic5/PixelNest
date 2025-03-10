using Microsoft.Data.SqlClient;
using PixelNestBackend.Data;
using System.Reflection.Metadata;

namespace PixelNestBackend.Utility
{
    public class UserUtility { 


        private readonly DataContext _dataContext;
        public UserUtility(DataContext dataContext)
        {
             _dataContext = dataContext;
        }
        public string GetClientGuid(string parameter)
        {
            string userID = "";
            if (_dataContext != null)
            {
                var user = _dataContext.Users.FirstOrDefault(x => (x.UserGuid).ToString() == parameter);
                if (user != null)
                {
                    userID = user.ClientGuid.ToString();
                }

            }
            return userID;
        }
        public Guid GetUserID(string parameter)
        {
            Guid userID = Guid.Empty;
            if (_dataContext != null)
            {
                var user =_dataContext.Users.FirstOrDefault(x => (x.Username == parameter) || ((x.ClientGuid).ToString() == parameter));
                if(user != null)
                {
                    userID = user.UserGuid;
                }

            }
            
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
        public string GetUserName(string userGuid)
        {

            if (_dataContext != null)
            {
                var user = _dataContext.Users.FirstOrDefault(x => (x.UserGuid).ToString() == userGuid || (x.ClientGuid).ToString() == userGuid);
                if (user != null)
                {
                    return user.Username;
                }


            }
            return null;

        }
        public string GetEmail(string userGuid)
        {

            if (_dataContext != null)
            {
                var user = _dataContext.Users.FirstOrDefault(x => (x.UserGuid).ToString() == userGuid);
                if (user != null)
                {
                    return user.Email;
                }


            }
            return null;

        }

    }
}
