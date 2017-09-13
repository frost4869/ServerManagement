using ServerManagement.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ServerManagement.View
{
    /// <summary>
    /// Interaction logic for Servers.xaml
    /// </summary>
    public partial class Server : UserControl
    {
        public ServerViewModel viewModel = new ServerViewModel();
        public Server()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = viewModel;
        }

        private void headerCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            viewModel.CheckAll();
            this.DataContext = viewModel;
        }

        private void headerCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            viewModel.UnCheckAll();
            this.DataContext = viewModel;
        }
    }
}
