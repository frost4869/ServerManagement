using ServerManagement.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerManagement.Identity
{
    public interface IAuthenticationService
    {
        User AuthenticateUser(string username, string password);
    }
    class AuthenticationService : IAuthenticationService
    {
        public User AuthenticateUser(string username, string password)
        {
            ServerManagementEntities db = new ServerManagementEntities();
            User user = db.Users.FirstOrDefault(q => q.Username.Equals(username) && q.Password.Equals(password));
            if(user == null)
            {
                throw new UnauthorizedAccessException("Wrong Username or Password. Please try again");
            }

            return user;
        }
    }
}
