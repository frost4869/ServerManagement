using AutoMapper;
using ServerManagement.Model;
using ServerManagement.Model.Entity;
using ServerManagement.ViewModel;
using ServerManagement.VML;
using System.Windows;
using System.Windows.Controls;

namespace ServerManagement.View
{
    /// <summary>
    /// Interaction logic for NewServer.xaml
    /// </summary>
    public partial class NewServer : UserControl
    {
        public ServerModel newServer = new ServerModel();
        private readonly ServerManagementEntities db = new ServerManagementEntities();

        public NewServer()
        {
            InitializeComponent();
            this.DataContext = newServer;
        }

        private async void ButtonSave_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                Crypto c = new Crypto(Crypto.CryptoTypes.encTypeTripleDES);

                ServerModel updatedServer = ((FrameworkElement)sender).DataContext as ServerModel;
                var entity = db.Servers.Find(updatedServer.Id);
                if (entity == null)//create server
                {
                    newServer.Password = c.Encrypt(newServer.Password);
                    newServer.Active = true;
                    entity = Mapper.Map<Model.Entity.Server>(newServer);
                    db.Servers.Add(entity);
                }
                else//update server
                {
                    updatedServer.Password = c.Encrypt(updatedServer.Password);
                    db.Entry(entity).CurrentValues.SetValues(Mapper.Map<Model.Entity.Server>(updatedServer));
                    db.Entry(entity).State = System.Data.Entity.EntityState.Modified;
                }
                
                db.SaveChanges();
                
                Server.Instance.DataContext = new ServerViewModel();
            }
            catch (System.Exception ex)
            {
                Utils.WriteLog(ex.Message);
                await Utils.ShowMessageBoxAsync("Error", ex.Message);
            }
        }
    }
}
