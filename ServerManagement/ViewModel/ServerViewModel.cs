using AutoMapper;
using AutoMapper.QueryableExtensions;
using ServerManagement.Model;
using ServerManagement.Model.Entity;
using ServerManagement.VML;
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
        private readonly ServerManagementEntities db = new ServerManagementEntities();
        public ObservableCollection<ServerModel> Servers { get; set; }

        public void LoadServers()
        {
            ObservableCollection<ServerModel> model = new ObservableCollection<ServerModel>();
            model = new ObservableCollection<ServerModel>(
                db.Servers.Where(q => q.Active)
                .ProjectTo<ServerModel>(Mapper.Configuration)
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

        public bool IsAnySelected()
        {
            if(Servers.Any(q => q.IsSelected))
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
