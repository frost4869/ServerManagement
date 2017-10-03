using AutoMapper;
using AutoMapper.QueryableExtensions;
using ServerManagement.Model;
using ServerManagement.Model.Entity;
using ServerManagement.VML;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace ServerManagement.ViewModel
{
    public class ServerViewModel
    {
        public ServerViewModel()
        {
            LoadServers();
        }
        public ServerViewModel(string keyWord)
        {
            Get(keyWord);
        }
        private readonly ServerManagementEntities db = new ServerManagementEntities();
        public ObservableCollection<ServerModel> Servers { get; set; }

        public void LoadServers()
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
                            i.MacAddressModel = Mapper.Map<MacAddressModel>(mac);
                            return i;
                        })
                        .AsQueryable();
                    q.IpAddresses = new ObservableCollection<IpModel>(IpModelList);
                    return q;
                });


            model = new ObservableCollection<ServerModel>(serverList);
            Servers = model;
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

        public void Get(string keyWord)
        {
            keyWord = keyWord.ToLower();
            Crypto c = new Crypto(Crypto.CryptoTypes.encTypeTripleDES);

            var totalResult = new List<ServerModel>();

            var result = db.Servers.Where(q => q.Active)
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
                            i.MacAddressModel = Mapper.Map<MacAddressModel>(mac);
                            return i;
                        })
                        .AsQueryable();
                    q.IpAddresses = new ObservableCollection<IpModel>(IpModelList);
                    return q;
                })
                .ToList();

            var filterByNameResult = result.Where(q => q.Name.ToLower().Contains(keyWord));
            if(filterByNameResult.Count() > 0)
                totalResult.AddRange(filterByNameResult);

            var filterByIpResult = result.Where(q => q.IpAddresses.Any(i => i.IPAddress.ToLower().Contains(keyWord))).ToList();
            if(filterByIpResult.Count() > 0)
                totalResult.AddRange(filterByIpResult);

            var filterByMacResult = result.Where(q => q.IpAddresses.Any(i => i.MacAddressModel.MacAddress1.ToLower().Contains(keyWord))).ToList();
            if (filterByMacResult.Count() > 0)
                totalResult.AddRange(filterByMacResult);

            Servers = new ObservableCollection<ServerModel>(totalResult);
        }

        public async Task DeleteSelected()
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
            }
            catch (System.Exception ex)
            {
                Utils.WriteLog(ex.Message);
                await Utils.ShowMessageBoxAsync("Error", ex.Message);
            }
        }

    }
}
