using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using ServerManagement.Identity;
using ServerManagement.Model.Entity;
using ServerManagement.View;
using ServerManagement.VML;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ServerManagement.ViewModel
{
    public interface IViewModel { }
    public class AuthenticationViewModel : IViewModel, INotifyPropertyChanged
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly DelegateCommand _loginCommand;
        private readonly DelegateCommand _logoutCommand;
        private readonly DelegateCommand _registerCommand;
        private readonly DelegateCommand _updateAdminPasswordCommand;
        private bool _isAdmin = false;
        private string _username;
        private string _confirmPassword;
        private string _password;
        private string _oldPassword;
        private Enums _roleEnum;
        private string _status;
        private Brush _statusColor;

        public ObservableCollection<User> Accounts { get; set; }
        public void LoadAccount()
        {
            ServerManagementEntities db = new ServerManagementEntities();
            CustomPrincipal customPrincipal = Thread.CurrentPrincipal as CustomPrincipal;
            var accounts = db.Users.Where(q => q.Username != customPrincipal.Identity.Name);
            Accounts = new ObservableCollection<User>(accounts);
        }

        public AuthenticationViewModel()
        {
            LoadAccount();
        }

        public AuthenticationViewModel(User user)
        {
            Username = user.Username;
            Role_Enum = (Enums)Enum.Parse(typeof(Enums), user.Role.RoleName, true);
        }

        public AuthenticationViewModel(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
            _loginCommand = new DelegateCommand(Login, CanLogin);
            _logoutCommand = new DelegateCommand(Logout, CanLogout);
            _registerCommand = new DelegateCommand(Register, CanRegister);
            _updateAdminPasswordCommand = new DelegateCommand(UpdateAdminPassword, CanUpdateAdminPassword);
        }

        #region Properties
        public bool IsAdmin
        {
            get
            {
                return (Thread.CurrentPrincipal as CustomPrincipal).IsInRole("Admin");
            }
        }
        public string Username
        {
            get { return _username; }
            set { _username = value; NotifyPropertyChanged("Username"); }
        }
        public string Password
        {
            get { return _password; }
            set { _password = value; NotifyPropertyChanged("Password"); }
        }
        public string ConfirmPassword
        {
            get { return _confirmPassword; }
            set { _confirmPassword = value; NotifyPropertyChanged("ConfirmPassword"); }
        }
        public string OldPassword
        {
            get
            {
                return _oldPassword;
            }
            set
            {
                _oldPassword = value;
                NotifyPropertyChanged("OldPassword");
            }
        }
        public Enums Role_Enum
        {
            get { return _roleEnum; }
            set { _roleEnum = value; NotifyPropertyChanged("RoleEnum"); }
        }
        public string Status
        {
            get { return _status; }
            set { _status = value; NotifyPropertyChanged("Status"); }
        }
        public string AuthenticatedUser
        {
            get
            {
                if (IsAuthenticated)
                    return Thread.CurrentPrincipal.Identity.Name;
                return "Not authenticated!";
            }
        }
        public Brush StatusColor
        {
            get
            {
                return _statusColor;
            }
            set
            {
                _statusColor = value;
                NotifyPropertyChanged("StatusColor");
            }
        }
        #endregion

        #region Commands
        public DelegateCommand LoginCommand { get { return _loginCommand; } }
        public DelegateCommand LogoutCommand { get { return _logoutCommand; } }
        public DelegateCommand RegisterCommand { get { return _registerCommand; } }
        public DelegateCommand UpdateAdminPasswordCommand { get { return _updateAdminPasswordCommand; } }
        #endregion

        private void Login(object parameter)
        {
            PasswordBox passwordBox = parameter as PasswordBox;
            string clearTextPassword = PasswordEncriptionHelper.HashPassword(passwordBox.Password);
            try
            {
                //Validate credentials through the authentication service
                User user = _authenticationService.AuthenticateUser(Username, clearTextPassword);

                //Get the current principal object
                CustomPrincipal customPrincipal = Thread.CurrentPrincipal as CustomPrincipal;
                if (customPrincipal == null)
                    throw new ArgumentException("The application's default thread principal must be set to a CustomPrincipal object on startup.");

                //Authenticate the user
                customPrincipal.Identity = new CustomIdentity(user.Username, user.Role.RoleName, user.Id);
                VML.Utils.RecordActivityAsync(ActivityType.Login, "User", DateTime.Now, user.Id, user.Id);

                //Update UI
                NotifyPropertyChanged("AuthenticatedUser");
                NotifyPropertyChanged("IsAuthenticated");
                _loginCommand.RaiseCanExecuteChanged();
                _logoutCommand.RaiseCanExecuteChanged();
                Username = string.Empty; //reset
                passwordBox.Password = string.Empty; //reset
                Status = string.Empty;
                AuthenticationViewModel viewModel = new AuthenticationViewModel(new AuthenticationService());
                if (customPrincipal.IsInRole("Reader"))
                {
                    IView readerWindow = new ReaderWindow(viewModel);
                    readerWindow.Show();
                }
                else
                {
                    IView main = new MainWindow(viewModel);
                    main.Show();
                }
                LoginWindow.Instance.Close();
            }
            catch (UnauthorizedAccessException ex)
            {
                StatusColor = Brushes.Red;
                Status = ex.Message;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private bool CanLogin(object parameter)
        {
            return !IsAuthenticated;
        }

        private void Logout(object parameter)
        {
            CustomPrincipal customPrincipal = Thread.CurrentPrincipal as CustomPrincipal;

            if (customPrincipal != null)
            {
                AuthenticationViewModel viewModel = new AuthenticationViewModel(new AuthenticationService());
                IView loginWindow = new LoginWindow(viewModel);
                loginWindow.Show();

                if (customPrincipal.IsInRole("Reader"))
                {
                    ReaderWindow.Instance.Close();
                }
                else
                {
                    MainWindow.Instance.Close();
                }

                VML.Utils.RecordActivityAsync(ActivityType.Logout, "User", DateTime.Now, customPrincipal.Identity.UserId, customPrincipal.Identity.UserId);

                customPrincipal.Identity = new UnauthenticatedIdentity();
                NotifyPropertyChanged("AuthenticatedUser");
                NotifyPropertyChanged("IsAuthenticated");
                _loginCommand.RaiseCanExecuteChanged();
                _logoutCommand.RaiseCanExecuteChanged();
                Status = string.Empty;
            }
        }
        private bool CanLogout(object parameter)
        {
            return IsAuthenticated;
        }

        private void Register(object parameter)
        {
            try
            {
                _authenticationService.Register(Username, Password, ConfirmPassword, Role_Enum);
                StatusColor = Brushes.Green;
                Status = "User Registed Successfully";
            }
            catch (Exception ex)
            {
                StatusColor = Brushes.Red;
                Status = ex.Message;
            }
        }
        private bool CanRegister(object parameter)
        {
            return IsAuthenticated;
        }

        private async void UpdateAdminPassword(object parameter)
        {
            try
            {
                if (IsAdmin)
                {
                    if (!string.IsNullOrEmpty(OldPassword) || !string.IsNullOrEmpty(Password) || !string.IsNullOrEmpty(ConfirmPassword))
                    {
                        var currentUser = (Thread.CurrentPrincipal as CustomPrincipal);
                        using (ServerManagementEntities db = new ServerManagementEntities())
                        {
                            using (var trans = db.Database.BeginTransaction())
                            {
                                User user = db.Users.FirstOrDefault(q => q.Username.Equals(currentUser.Identity.Name));
                                if (user != null)
                                {
                                    _authenticationService.UpdateAdminPassword(user.Id, OldPassword, Password, ConfirmPassword);
                                    var metroWindow = (Application.Current.Windows.OfType<Window>().SingleOrDefault(q => q.IsActive) as MetroWindow);
                                    MessageDialogStyle style = MessageDialogStyle.Affirmative;
                                    var result = await metroWindow.ShowMessageAsync("Logout", "Please login again with the new Password ", style);
                                    if (result == MessageDialogResult.Affirmative)
                                    {
                                        this.Logout(null);
                                        UpdateAdminPasswordWindow.Instance.Close();
                                    }
                                }
                                else
                                {
                                    StatusColor = Brushes.Red;
                                    Status = "User information not found!";
                                }
                            }
                        }
                    }
                    else
                    {
                        StatusColor = Brushes.Red;
                        Status = "Please provide required Information";
                    }
                }
                else
                {
                    StatusColor = Brushes.Red;
                    Status = "Current user is not an Administrator";
                }
            }
            catch (Exception ex)
            {
                StatusColor = Brushes.Red;
                Status = ex.Message;
            }
        }
        private bool CanUpdateAdminPassword(object parameter)
        {
            return IsAdmin && IsAuthenticated;
        }

        public bool IsAuthenticated
        {
            get { return Thread.CurrentPrincipal.Identity.IsAuthenticated; }
        }

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
