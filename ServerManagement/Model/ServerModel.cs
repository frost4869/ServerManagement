using ServerManagement.Model.Entity;
using System.ComponentModel;

namespace ServerManagement.Model
{
    public class ServerModel : Server, INotifyPropertyChanged
    {
        private bool mIsSelected;
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
        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
