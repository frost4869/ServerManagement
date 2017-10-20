using ServerManagement.Identity;
using ServerManagement.Model.Entity;
using ServerManagement.VML;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows.Media;

namespace ServerManagement.ViewModel
{
    public class AccountViewModel : INotifyPropertyChanged
    {
        public AccountViewModel(User sourceUser)
        {
            _updateUserCommand = new DelegateCommand(UpdateAccount, CanUpdateAccount);

            Id = sourceUser.Id;
            Username = sourceUser.Username;
            Enums sourceUserRole;
            if(Enum.TryParse(sourceUser.Role.RoleName, true, out sourceUserRole))
            {
                CurrentRole = sourceUserRole;
            }
        }

        private int _id;
        private string _username;
        private string _password;
        private string _confirmPassword;
        private string _status;
        private Brush _statusColor;

        public int Id
        {
            get { return _id; }
            set { _id = value; NotifyPropertyChanged("Id"); }
        }
        public string Username
        {
            get
            {
                return _username;
            }
            set
            {
                _username = value;
                NotifyPropertyChanged("Username");
            }
        }
        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
                NotifyPropertyChanged("Password");
            }
        }
        public string ConfirmPassword
        {
            get
            {
                return _confirmPassword;
            }
            set
            {
                _confirmPassword = value;
                NotifyPropertyChanged("ConfirmPassword");
            }
        }
        public string Status
        {
            get { return _status; }
            set { _status = value; NotifyPropertyChanged("Status"); }
        }
        public Brush StatusColor
        {
            get { return _statusColor; }
            set
            {
                _statusColor = value;
                NotifyPropertyChanged("StatusColor");
            }
        }

        private Enums _currentRole;
        public Enums CurrentRole
        {
            get
            {
                return _currentRole;
            }
            set
            {
                if (_currentRole == value)
                    return;
                _currentRole = value;
                NotifyPropertyChanged("CurrentRole");
            }
        }

        private readonly DelegateCommand _updateUserCommand;
        public DelegateCommand UpdateAccountCommand { get { return _updateUserCommand; } }
        private void UpdateAccount(object parameter)
        {
            try
            {
                AuthenticationService service = new AuthenticationService();
                service.UpdateAccount(Id, Password, ConfirmPassword, CurrentRole);
                StatusColor = Brushes.Green;
                Status = "User updated successfully";
            }
            catch (Exception ex)
            {
                StatusColor = Brushes.Red;
                Status = ex.Message;
            }
        }
        private bool CanUpdateAccount(object parameter)
        {
            return IsAuthenticated;
        }

        public bool IsAuthenticated
        {
            get { return Thread.CurrentPrincipal.Identity.IsAuthenticated; }
        }

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
