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
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = new ServerViewModel();
        }

        private async void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            await Task.Delay(1000);
            var keyWord = searchBox.Text;
            if (!string.IsNullOrEmpty(keyWord))
            {
                var viewModel = new ServerViewModel(keyWord);
                this.DataContext = viewModel;
            }
            else
            {
                this.DataContext = new ServerViewModel();
            }
        }
    }
}
