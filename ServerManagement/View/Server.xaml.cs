using MahApps.Metro.Controls;
using ServerManagement.Model;
using ServerManagement.ViewModel;
using ServerManagement.VML;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

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
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = new ServerViewModel();
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
            this.DataContext = new ServerViewModel();
        }

        private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(((DataGrid)sender).SelectedCells.Count > 0)
            {
                ServerModel server = ((DataGrid)sender).SelectedCells[0].Item as ServerModel;
                DetailsView.DataContext = server;
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
                var viewModel = new ServerViewModel();
                viewModel.Get(keyWord);
                this.DataContext = viewModel;
            }
            else
            {
                this.DataContext = new ServerViewModel();
            }
        }
    }
}
