using ServerManagement.Model.Entity;
using System.ComponentModel;

namespace ServerManagement.Model
{
    public class ServerModel : INotifyPropertyChanged
    {
        private bool mIsSelected;
        private int mId;
        private string mName;
        private string mHostName;
        private string mIP1;
        private string mIP2;
        private string mIP3;
        private string mIP4;
        private string mUsername;
        private string mPassword;
        private string mFunctionality;
        private string mProject;
        private bool mActive;
        public bool IsSelected
        {
            get
            {
                return mIsSelected;
            }
            set
            {
                mIsSelected = value;
                RaisePropertyChanged("IsSelected");
            }
        }
        public int Id
        {
            get
            {
                return this.mId;
            }
            set
            {
                this.mId = value;
                RaisePropertyChanged("Id");
            }
        }
        public string Name
        {
            get
            {
                return this.mName;
            }
            set
            {
                if (value != this.mName)
                {
                    mName = value;
                    RaisePropertyChanged("Name");
                }
            }
        }
        public string HostName {
            get
            {
                return this.mHostName;
            }
            set
            {
                if (value != this.mHostName)
                {
                    mHostName = value;
                    RaisePropertyChanged("HostName");
                }
            }
        }
        public string IP1 {
            get
            {
                return this.mIP1;
            }
            set
            {
                if (value != this.mIP1)
                {
                    mIP1 = value;
                    RaisePropertyChanged("IP1");
                }
            }
        }
        public string IP2 {
            get
            {
                return this.mIP2;
            }
            set
            {
                if (value != this.mIP2)
                {
                    mIP2 = value;
                    RaisePropertyChanged("IP2");
                }
            }
        }
        public string IP3 {
            get
            {
                return this.mIP3;
            }
            set
            {
                if (value != this.mIP3)
                {
                    mIP3 = value;
                    RaisePropertyChanged("IP3");
                }
            }
        }
        public string IP4 {
            get
            {
                return this.mIP4;
            }
            set
            {
                if (value != this.mIP4)
                {
                    mIP4 = value;
                    RaisePropertyChanged("IP4");
                }
            }
        }
        public string Username {
            get
            {
                return this.mUsername;
            }
            set
            {
                if (value != this.mUsername)
                {
                    mUsername = value;
                    RaisePropertyChanged("Username");
                }
            }
        }
        public string Password {
            get
            {
                return this.mPassword;
            }
            set
            {
                if (value != this.mPassword)
                {
                    mPassword = value;
                    RaisePropertyChanged("Password");
                }
            }
        }
        public string Functionality {
            get
            {
                return this.mFunctionality;
            }
            set
            {
                if (value != this.mFunctionality)
                {
                    mFunctionality = value;
                    RaisePropertyChanged("Functionality");
                }
            }
        }
        public string Project {
            get
            {
                return this.mProject;
            }
            set
            {
                if (value != this.mProject)
                {
                    mProject = value;
                    RaisePropertyChanged("Project");
                }
            }
        }
        public bool Active {
            get
            {
                return this.mActive;
            }
            set
            {
                if (value != this.mActive)
                {
                    mActive = value;
                    RaisePropertyChanged("Active");
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
