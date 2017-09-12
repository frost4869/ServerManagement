using ServerManagement.Model.Entity;
using System.ComponentModel;

namespace ServerManagement.Model
{
    class ServerModel : Server, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
    }
}
