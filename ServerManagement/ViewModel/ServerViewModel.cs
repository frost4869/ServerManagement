using AutoMapper;
using AutoMapper.QueryableExtensions;
using ServerManagement.Model;
using ServerManagement.Model.Entity;
using System.Collections.ObjectModel;
using System.Linq;

namespace ServerManagement.ViewModel
{
    public class ServerViewModel
    {
        public ServerViewModel()
        {
            LoadServers();
        }
        private readonly ServerManagementEntities db = new ServerManagementEntities();
        private MapperConfiguration config = new MapperConfiguration(q =>
        {
            q.CreateMap<Server, ServerModel>();
            q.CreateMap<ServerModel, Server>();
        });
        public ObservableCollection<ServerModel> Servers { get; set; }

        public void LoadServers()
        {
            ObservableCollection<ServerModel> model = new ObservableCollection<ServerModel>();
            model = new ObservableCollection<ServerModel>(
                db.Servers.Where(q => q.Active)
                .ProjectTo<ServerModel>(config)
                .ToList()
            );

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
    }
}
