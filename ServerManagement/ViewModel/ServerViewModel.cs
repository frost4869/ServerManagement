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

namespace ServerManagement.ViewModel
{
    public class ServerViewModel : INotifyPropertyChanged
    {
        public ServerViewModel()
        {
            //LoadServersAsync();
        }
        public ServerViewModel(string keyWord)
        {
            Get(keyWord);
        }
        private readonly ServerManagementEntities db = new ServerManagementEntities();
        private ObservableCollection<ServerModel> _servers;

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
                        var IpModelList = db.IPs.Where(i => i.ServerId == q.Id && i.Active)
                            .ProjectTo<IpModel>(Mapper.Configuration)
                            .ToList()
                            .Select(i =>
                            {
                                var mac = db.MacAddresses.Find(i.MacAddressId);
                                if (mac != null)
                                {
                                    i.MacAddressModel = Mapper.Map<MacAddressModel>(mac);
                                }
                                return i;
                            })
                            .AsQueryable();
                        q.IpAddresses = new ObservableCollection<IpModel>(IpModelList);
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

        public async Task Get(string keyWord)
        {
            await Task.Run(() =>
            {
                keyWord = keyWord.ToLower();


                //Servers = new ObservableCollection<ServerModel>(Servers.Where(q => q.Name.ToLower().Contains(keyWord) ||
                //                           q.HostName.ToLower().Contains(keyWord) ||
                //                           q.Project.ToLower().Contains(keyWord) ||
                //                           q.IpAddresses.Any(i => i.IPAddress.ToLower().Contains(keyWord)) ||
                //                           q.IpAddresses.Any(i => i.MacAddressModel == null ? i.MacAddressModel.MacAddress1.ToLower().Contains(keyWord) : false)));
            });
        }

        public async Task DeleteSelected()
        {
            using (ServerManagementEntities db = new ServerManagementEntities())
            {
                using(var transaction = db.Database.BeginTransaction())
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
