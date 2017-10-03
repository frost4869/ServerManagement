using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerManagement.Model
{
    public class MacAddressModel : INotifyPropertyChanged
    {
        private int _id;
        private string _macAddress;
        private bool _active;

        public int Id
        {
            get { return _id; }
            set { _id = value; RaisePropertyChanged("Id"); }
        }
        public string MacAddress1
        {
            get
            {
                return _macAddress;
            }
            set
            {
                _macAddress = value;
                RaisePropertyChanged("MacAddress1");
            }
        }
        public bool Active
        {
            get
            {
                return _active;
            }
            set
            {
                _active = value;
                RaisePropertyChanged("Active");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
