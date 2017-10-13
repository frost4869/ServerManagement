using ServerManagement.Model;
using ServerManagement.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for ReaderServerView.xaml
    /// </summary>
    public partial class ReaderServerView : UserControl
    {
        public static ReaderServerView Instance
        {
            get;
            private set;
        }
        public ReaderServerView()
        {
            InitializeComponent();
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
