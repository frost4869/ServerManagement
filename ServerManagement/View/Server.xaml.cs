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
using System.Windows.Threading;

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

            LoadData();
        }

        private void LoadData()
        {
            ProgressPanel.Visibility = Visibility.Visible;
            var viewModel = new ServerViewModel();
            this.DataContext = viewModel;

            viewModel.LoadServersAsync()
                .ContinueWith(_ =>
                {
                    GetCustomColumnsAsync(viewModel.Servers);
                });
        }

        private async Task GetCustomColumnsAsync(ObservableCollection<ServerModel> serverList)
        {
            await Task.Run(() =>
            {
                Application.Current.Dispatcher.Invoke((Action)(() =>
                {
                    foreach (var server in serverList)
                    {
                        var columns = new List<DataGridColumn>();
                        for (int i = 0; i < server.IpAddresses.Count; i++)
                        {
                            var newColumn = new DataGridTextColumn()
                            {
                                Header = "IP " + (i + 1),
                                Width = new DataGridLength(1.0, DataGridLengthUnitType.Auto)
                            };
                            Binding valueBinding = new Binding()
                            {
                                Path = new PropertyPath("IpAddresses[" + i + "].IPAddress"),
                                Mode = BindingMode.TwoWay,
                                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                                NotifyOnSourceUpdated = true,
                                NotifyOnTargetUpdated = true
                            };
                            newColumn.Binding = valueBinding;

                            if (!CheckIfColumnAdded(newColumn.Header.ToString()))
                            {
                                serverDataGrid.Columns.Add(newColumn);
                            }
                        }
                    }

                    ProgressPanel.Visibility = Visibility.Collapsed;
                }));
            });
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
            LoadData();
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
            var keyWord = searchBox.Text.ToLower();

            await Task.Run(new Action(() =>
            {
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    var view = CollectionViewSource.GetDefaultView((DataContext as ServerViewModel).Servers);
                    view.Filter = o => (o as ServerModel).Name.ToLower().Contains(keyWord) ||
                                        (o as ServerModel).HostName.ToLower().Contains(keyWord) ||
                                        (o as ServerModel).Project.ToLower().Contains(keyWord) ||
                                        (o as ServerModel).IpAddresses.Any(i => i.IPAddress != null ? 
                                                    i.IPAddress.ToLower().Contains(keyWord) : false) ||
                                        (o as ServerModel).IpAddresses.Any(i => i.MacAddressModel != null ? 
                                                    i.MacAddressModel.MacAddress1.ToLower().Contains(keyWord) : false);
                }));
            }));
        }

        private void ButtonRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void CopyButton_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(((PasswordBox)((Grid)((Button)sender).Parent).FindName("txtPassword")).Password);
        }

        private bool IsMatch(object o)
        {
            ServerModel server = o as ServerModel;

            return (DataContext as ServerViewModel).Servers.Any(q => q.Name.ToLower().Contains(searchBox.Text.ToLower()));
        }
    }
}
