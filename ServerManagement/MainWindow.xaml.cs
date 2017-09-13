using MahApps.Metro.Controls;
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

namespace ServerManagement
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void NewServer_Click(object sender, RoutedEventArgs e)
        {
            MetroTabItem item = new MetroTabItem
            {
                Header = "New Server",
                CloseButtonEnabled = true,
            };

            View.NewServer view = new View.NewServer();
            item.Content = view;

            MainTabControl.Items.Add(item);
            item.Focus();
        }

        private void MainTabControl_TabItemClosingEvent(object sender, BaseMetroTabControl.TabItemClosingEventArgs e)
        {
            if (e.ClosingTabItem.Header.ToString().StartsWith("sizes"))
                e.Cancel = true;
        }
    }
}
