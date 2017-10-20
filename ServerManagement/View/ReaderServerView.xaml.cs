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

            viewModel.LoadServersAsync().ContinueWith(_ =>
            {
                this.Dispatcher.Invoke(new Action(() =>
                {
                    ProgressPanel.Visibility = Visibility.Hidden;
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

        private bool IsMatch(object o)
        {
            ServerModel server = o as ServerModel;

            return (DataContext as ServerViewModel).Servers.Any(q => q.Name.ToLower().Contains(searchBox.Text.ToLower()));
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
