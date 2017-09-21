using ServerManagement.Model.Entity;
using ServerManagement.VML;
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
        void Register(string username, string password, string confirmPassword, RoleEnum role);
    }
    class AuthenticationService : IAuthenticationService
    {
        public User AuthenticateUser(string username, string password)
        {
            ServerManagementEntities db = new ServerManagementEntities();
            User user = db.Users.FirstOrDefault(q => q.Username.Equals(username) && q.Password.Equals(password));
            if (user == null)
            {
                throw new UnauthorizedAccessException("Wrong Username or Password. Please try again");
            }

            return user;
        }

        public void Register(string username, string password, string confirmPassword, RoleEnum role)
        {
            ServerManagementEntities db = new ServerManagementEntities();
            if (!string.IsNullOrEmpty(username) || !string.IsNullOrEmpty(password))
            {
                if (password.Equals(confirmPassword))
                {
                    if (!db.Users.Any(q => q.Username.Equals(username)))
                    {
                        switch (role)
                        {
                            case RoleEnum.Admin:
                                {
                                    db.Users.Add(new User
                                    {
                                        Active = true,
                                        Username = username,
                                        Password = PasswordEncriptionHelper.HashPassword(password),
                                        RoleId = 1
                                    });
                                }
                                break;
                            case RoleEnum.Writer:
                                {
                                    db.Users.Add(new User
                                    {
                                        Active = true,
                                        Username = username,
                                        Password = PasswordEncriptionHelper.HashPassword(password),
                                        RoleId = 2
                                    });
                                }
                                break;
                            case RoleEnum.Reader:
                                {
                                    db.Users.Add(new User
                                    {
                                        Active = true,
                                        Username = username,
                                        Password = PasswordEncriptionHelper.HashPassword(password),
                                        RoleId = 3
                                    });
                                }
                                break;
                            default:
                                break;
                        }
                        db.SaveChanges();
                    }
                    else
                    {
                        throw new Exception("Username already existed !");
                    }
                }
                else
                {
                    throw new Exception("Confirm Password Mismatched !");
                }
            }
            else
            {
                throw new Exception("Please provide Username and Password");
            }
        }
    }
}
