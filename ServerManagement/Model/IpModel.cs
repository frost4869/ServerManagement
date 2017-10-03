using ServerManagement.Model.Entity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerManagement.Model
{
    public class IpModel : INotifyPropertyChanged
    {
        private int _id;
        private string _ipAddress;
        private int _serverId;
        private bool _active;
        private int _macId;
        private MacAddressModel _macAddressModel;
        private bool _isRemoved = false;

        public int Id
        {
            get { return _id; }
            set { _id = value; RaisePropertyChanged("Id"); }
        }
        public string IPAddress
        {
            get { return _ipAddress; }
            set { _ipAddress = value; RaisePropertyChanged("IPAddress"); }
        }
        public int ServerId
        {
            get { return _serverId; }
            set { _serverId = value; RaisePropertyChanged("ServerId"); }
        }
        public bool Active
        {
            get { return _active; }
            set { _active = value; RaisePropertyChanged("Active"); }
        }

        public int MacAddressId
        {
            get { return _macId; }
            set { _macId = value; RaisePropertyChanged("MacAddressId"); }
        }

        public MacAddressModel MacAddressModel
        {
            get { return _macAddressModel; }
            set { _macAddressModel = value; RaisePropertyChanged("MacAddressModel"); }
        }

        public bool IsRemoved
        {
            get { return _isRemoved; }
            set { _isRemoved = value; RaisePropertyChanged("IsRemoved"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
