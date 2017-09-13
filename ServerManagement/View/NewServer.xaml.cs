using ServerManagement.Model.Entity;
using ServerManagement.VML;
using System.Windows.Controls;

namespace ServerManagement.View
{
    /// <summary>
    /// Interaction logic for NewServer.xaml
    /// </summary>
    public partial class NewServer : UserControl
    {
        public Model.Entity.Server newServer = new Model.Entity.Server();
        private readonly ServerManagementEntities db = new ServerManagementEntities();

        public NewServer()
        {
            InitializeComponent();
            NewServerGrid.DataContext = newServer;
        }

        private async void ButtonSave_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                newServer.Active = true;
                db.Servers.Add(newServer);
                db.SaveChanges();
            }
            catch (System.Exception ex)
            {
                Utils.WriteLog(ex.Message);
                await Utils.ShowMessageBoxAsync("Error", ex.Message);
            }
        }
    }
}
