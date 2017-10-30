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
        void Register(string username, string password, string confirmPassword, Enums role);
        void UpdateAccount(int Id, string password, string confirmPassword, Enums role, bool active);
        void UpdateAdminPassword(int id, string oldPassword, string newPassword, string confirmNewPassword);
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
            else if (!user.Active)
            {
                throw new UnauthorizedAccessException("User has been Deactivated by Admin.");
            }

            return user;
        }

        public void Register(string username, string password, string confirmPassword, Enums role)
        {
            using (ServerManagementEntities db = new ServerManagementEntities())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    if (!string.IsNullOrEmpty(username) || !string.IsNullOrEmpty(password))
                    {
                        if (password.Equals(confirmPassword))
                        {
                            if (!db.Users.Any(q => q.Username.Equals(username)))
                            {
                                switch (role)
                                {
                                    case Enums.Admin:
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
                                    case Enums.Writer:
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
                                    case Enums.Reader:
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
                                transaction.Commit();
                            }
                            else
                            {
                                transaction.Rollback();
                                throw new Exception("Username already existed !");
                            }
                        }
                        else
                        {
                            transaction.Rollback();
                            throw new Exception("Confirm Password Mismatched !");
                        }
                    }
                    else
                    {
                        transaction.Rollback();
                        throw new Exception("Please provide Username and Password");
                    }
                }
            }
        }

        public void UpdateAccount(int Id, string password, string confirmPassword, Enums role, bool active)
        {
            using (ServerManagementEntities db = new ServerManagementEntities())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    User user = db.Users.Find(Id);
                    if (user != null)
                    {
                        try
                        {
                            if (password != null && confirmPassword != null)
                            {
                                if (password.Equals(confirmPassword))
                                {
                                    user.Password = PasswordEncriptionHelper.HashPassword(password);
                                }
                                else
                                {
                                    throw new Exception("Confirm Password miss matched");
                                }
                            }
                            switch (role)
                            {
                                case Enums.Admin:
                                    user.RoleId = 1;
                                    break;
                                case Enums.Writer:
                                    user.RoleId = 2;
                                    break;
                                case Enums.Reader:
                                    user.RoleId = 3;
                                    break;
                                default:
                                    break;
                            }
                            user.Active = active;
                            db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw ex;
                        }
                    }
                    else
                    {
                        throw new Exception("User do not existed");
                    }
                }
            }
        }

        public void UpdateAdminPassword(int id, string oldPassword, string newPassword, string confirmNewPassword)
        {
            using (ServerManagementEntities db = new ServerManagementEntities())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    User user = db.Users.Find(id);
                    if (user != null)
                    {
                        if (user.Password.Equals(PasswordEncriptionHelper.HashPassword(oldPassword)))
                        {
                            if (newPassword.Equals(confirmNewPassword))
                            {
                                user.Password = PasswordEncriptionHelper.HashPassword(newPassword);
                                db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                                db.SaveChanges();
                                transaction.Commit();
                            }
                            else
                            {
                                transaction.Rollback();
                                throw new Exception("Confirm New Password miss matched!");
                            }
                        }
                        else
                        {
                            transaction.Rollback();
                            throw new Exception("Old Password is not matched!");
                        }
                    }
                    else
                    {
                        transaction.Rollback();
                        throw new Exception("User does not exist!");
                    }
                }
            }
        }
    }
}
