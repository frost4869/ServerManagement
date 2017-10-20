using AutoMapper;
using AutoMapper.QueryableExtensions;
using ServerManagement.Model;
using ServerManagement.Model.Entity;
using ServerManagement.VML;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ServerManagement.ViewModel
{
    public class ServerViewModel : INotifyPropertyChanged
    {
        private readonly ServerManagementEntities db = new ServerManagementEntities();
        private ObservableCollection<ServerModel> _servers;
        private DataGridRowDetailsVisibilityMode _rowDetailsVisibility;

        public DataGridRowDetailsVisibilityMode RowDetailsVisibility
        {
            get
            {
                return _rowDetailsVisibility;
            }
            set
            {
                _rowDetailsVisibility = value;
                RaisePropertyChanged("RowDetailsVisibility");
            }
        }
        public ObservableCollection<ServerModel> Servers
        {
            get
            {
                return _servers;
            }
            set
            {
                _servers = value;
                RaisePropertyChanged("Servers");
            }
        }

        public async Task LoadServersAsync()
        {
            await Task.Run(() =>
            {
                Crypto c = new Crypto(Crypto.CryptoTypes.encTypeTripleDES);
                ObservableCollection<ServerModel> model = new ObservableCollection<ServerModel>();
                var serverList = db.Servers.Where(q => q.Active)
                    .ProjectTo<ServerModel>(Mapper.Configuration)
                    .ToList()
                    .Select(q =>
                    {
                        q.Password = c.Decrypt(q.Password, Crypto.CryptoTypes.encTypeTripleDES);
                        var IpMng = db.IPs.FirstOrDefault(i => i.Active && i.ServerId == q.Id && i.Type == (int)IpType.Management);
                        var IpWan = db.IPs.FirstOrDefault(i => i.Active && i.ServerId == q.Id && i.Type == (int)IpType.Wan);
                        var IpLan = db.IPs.FirstOrDefault(i => i.Active && i.ServerId == q.Id && i.Type == (int)IpType.Lan);
                        var IpDB = db.IPs.FirstOrDefault(i => i.Active && i.ServerId == q.Id && i.Type == (int)IpType.Database);

                        if(IpMng != null)
                        {
                            q.IpMng = IpMng.IPAddress;
                            var macMng = db.MacAddresses.FirstOrDefault(m => m.Id == IpMng.MacAddressId && m.Active);
                            q.MacMng = macMng == null ? "" : macMng.MacAddress1;
                        }
                        if (IpWan != null)
                        {
                            q.IpWan = IpWan.IPAddress;
                            var macWan = db.MacAddresses.FirstOrDefault(m => m.Id == IpWan.MacAddressId && m.Active);
                            q.MacWan = macWan == null ? "" : macWan.MacAddress1;
                        }
                        if (IpLan != null)
                        {
                            q.IpLan = IpLan.IPAddress;
                            var macLan = db.MacAddresses.FirstOrDefault(m => m.Id == IpLan.MacAddressId && m.Active);
                            q.MacLan = macLan == null ? "" : macLan.MacAddress1;
                        }
                        if (IpDB != null)
                        {
                            q.IpDB = IpDB.IPAddress;
                            var macDb = db.MacAddresses.FirstOrDefault(m => m.Id == IpDB.MacAddressId && m.Active);
                            q.MacDB = macDb == null ? "" : macDb.MacAddress1;
                        }

                        return q;
                    });
                model = new ObservableCollection<ServerModel>(serverList);
                Servers = model;
            });
        }

        public void CheckAll()
        {
            foreach (var item in Servers)
            {
                item.IsSelected = true;
            }
        }

        public void UnCheckAll()
        {
            foreach (var item in Servers)
            {
                item.IsSelected = false;
            }
        }

        public bool IsAnySelected()
        {
            if (Servers.Any(q => q.IsSelected))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public async Task DeleteSelected()
        {
            using (ServerManagementEntities db = new ServerManagementEntities())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (var item in Servers)
                        {
                            if (item.IsSelected)
                            {
                                var server = await db.Servers.FindAsync(Mapper.Map<Server>(item).Id);
                                server.Active = false;
                            }
                        }

                        await db.SaveChangesAsync();
                        transaction.Commit();
                    }
                    catch (System.Exception ex)
                    {
                        transaction.Rollback();
                        Utils.WriteLog(ex.Message);
                        await Utils.ShowMessageBoxAsync("Error", ex.Message);
                    }
                }
            }
        }

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
