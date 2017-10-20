using AutoMapper;
using MahApps.Metro.Controls;
using ServerManagement.Model;
using ServerManagement.Model.Entity;
using ServerManagement.ViewModel;
using ServerManagement.VML;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

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

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void CopyButton_Click(object sender, RoutedEventArgs e)
        {
            if (newServer.Password != null)
            {
                Clipboard.SetText(newServer.Password);
            }
            else
            {
                ServerModel updatedServer = ((FrameworkElement)sender).DataContext as ServerModel;
                if(updatedServer.Password != null)
                {
                    Clipboard.SetText(updatedServer.Password);
                }
            }
        }
    }
}
