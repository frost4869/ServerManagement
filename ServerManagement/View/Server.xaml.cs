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

            viewModel.LoadServersAsync().ContinueWith(_ =>
            {
                this.Dispatcher.Invoke(new Action(() =>
                {
                    ProgressPanel.Visibility = Visibility.Hidden;
                }));
            });
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
            MessageBoxResult result = MessageBox.Show("Are you sure to delete these records ?", "Delete", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

            if(result == MessageBoxResult.Yes)
            {
                var dataContext = this.DataContext as ServerViewModel;
                await dataContext.DeleteSelected();
                LoadData();
            }
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
                    view.Filter = item =>
                    {
                        ServerModel model = item as ServerModel;
                        if (model == null)
                            return false;

                        bool flag = false;

                        if (model.Name != null)
                        {
                            if (model.Name.ToLower().Contains(keyWord))
                                flag = true;
                        }
                        if (model.HostName != null)
                        {
                            if (model.HostName.ToLower().Contains(keyWord))
                                flag = true;
                        }
                        if (model.Project != null)
                        {
                            if (model.Project.ToLower().Contains(keyWord))
                                flag = true;
                        }
                        if (model.IpMng != null)
                        {
                            if (model.IpMng.ToLower().Contains(keyWord))
                                flag = true;
                        }
                        if (model.IpWan != null)
                        {
                            if (model.IpWan.ToLower().Contains(keyWord))
                                flag = true;
                        }
                        if (model.IpLan != null)
                        {
                            if (model.IpLan.ToLower().Contains(keyWord))
                                flag = true;
                        }
                        if (model.IpDB != null)
                        {
                            if (model.IpDB.ToLower().Contains(keyWord))
                                flag = true;
                        }
                        if (model.MacMng != null)
                        {
                            if (model.MacMng.ToLower().Contains(keyWord))
                                flag = true;
                        }
                        if (model.MacWan != null)
                        {
                            if (model.MacWan.ToLower().Contains(keyWord))
                                flag = true;
                        }
                        if (model.MacLan != null)
                        {
                            if (model.MacLan.ToLower().Contains(keyWord))
                                flag = true;
                        }
                        if (model.MacDB != null)
                        {
                            if (model.MacDB.ToLower().Contains(keyWord))
                                flag = true;
                        }

                        return flag;
                    };
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

        private void serverDataGrid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var viewModel = this.DataContext as ServerViewModel;
            if (viewModel.RowDetailsVisibility == DataGridRowDetailsVisibilityMode.Collapsed)
            {
                viewModel.RowDetailsVisibility = DataGridRowDetailsVisibilityMode.VisibleWhenSelected;
            }
            else
            {
                viewModel.RowDetailsVisibility = DataGridRowDetailsVisibilityMode.Collapsed;
            }
        }
    }
}
