using MahApps.Metro.Controls;
using ServerManagement.Model;
using ServerManagement.ViewModel;
using ServerManagement.VML;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System;
using System.Globalization;
using System.Linq;

namespace ServerManagement.View
{
    /// <summary>
    /// Interaction logic for Servers.xaml
    /// </summary>
    public partial class Server : UserControl
    {
        public static Server Instance
        {
            get;
            private set;
        }
        public Server()
        {
            InitializeComponent();
            Instance = this;
            this.serverDataGrid.AddHandler(DataGridRow.MouseDoubleClickEvent,
                                        new MouseButtonEventHandler(DataGridRow_MouseDoubleClick), true);

            var viewModel = new ServerViewModel();
            this.DataContext = viewModel;
            var serverList = viewModel.Servers;
            GetCustomColumns(serverList);
        }

        private void GetCustomColumns(ObservableCollection<ServerModel> serverList)
        {
            foreach (var server in serverList)
            {
                var columns = new List<DataGridColumn>();
                for (int i = 0; i < server.IpAddresses.Count; i++)
                {
                    var newColumn = new DataGridTextColumn();
                    newColumn.Header = "IP " + (i + 1);
                    newColumn.Width = new DataGridLength(1.0, DataGridLengthUnitType.Auto);

                    Binding valueBinding = new Binding();
                    valueBinding.Path = new PropertyPath("IpAddresses[" + i + "].IPAddress");
                    valueBinding.Mode = BindingMode.TwoWay;
                    valueBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                    valueBinding.NotifyOnSourceUpdated = true;
                    valueBinding.NotifyOnTargetUpdated = true;

                    newColumn.Binding = valueBinding;

                    if (!CheckIfColumnAdded(newColumn.Header.ToString()))
                    {
                        serverDataGrid.Columns.Add(newColumn);
                    }
                }
            }
        }

        private bool CheckIfColumnAdded(string ColName)
        {
            return serverDataGrid.Columns.Any(q => q.Header.ToString() == ColName);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {


        }

        private void headerCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            btnDelete.Visibility = Visibility.Visible;
            var dataContext = this.DataContext as ServerViewModel;
            dataContext.CheckAll();
        }

        private void headerCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            btnDelete.Visibility = Visibility.Hidden;
            var dataContext = this.DataContext as ServerViewModel;
            dataContext.UnCheckAll();
        }

        private async void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            var dataContext = this.DataContext as ServerViewModel;
            await dataContext.DeleteSelected();
            var viewModel = new ServerViewModel();
            serverDataGrid.Columns.Clear();
            this.DataContext = viewModel;
            var serverList = viewModel.Servers;
            GetCustomColumns(serverList);
        }

        private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (((DataGrid)sender).SelectedCells.Count > 0)
            {
                ServerModel server = ((DataGrid)sender).SelectedCells[0].Item as ServerModel;
                MetroWindow updateServerWindow = new MetroWindow
                {
                    Title = "Server: " + server.Name,
                    EnableDWMDropShadow = true,
                    WindowStyle = WindowStyle.SingleBorderWindow,
                    Content = new NewServer()
                    {
                        DataContext = server,
                    },
                };
                updateServerWindow.SizeToContent = SizeToContent.Height;
                updateServerWindow.Show();
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            btnDelete.Visibility = Visibility.Visible;
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            var dataContext = this.DataContext as ServerViewModel;
            if (dataContext.IsAnySelected())
            {
                btnDelete.Visibility = Visibility.Hidden;
            }
        }

        private async void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            await Task.Delay(1000);
            var keyWord = searchBox.Text;
            if (!string.IsNullOrEmpty(keyWord))
            {
                var viewModel = new ServerViewModel(keyWord);
                this.DataContext = viewModel;
                var serverList = viewModel.Servers;
                GetCustomColumns(serverList);
            }
            else
            {
                var viewModel = new ServerViewModel();
                this.DataContext = viewModel;
                var serverList = viewModel.Servers;
                GetCustomColumns(serverList);
            }
        }

        private void ButtonRefresh_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = new ServerViewModel();
            this.DataContext = viewModel;
            var serverList = viewModel.Servers;
            GetCustomColumns(serverList);
        }

        private void CopyButton_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(((PasswordBox)((Grid)((Button)sender).Parent).FindName("txtPassword")).Password);
        }
    }
}
