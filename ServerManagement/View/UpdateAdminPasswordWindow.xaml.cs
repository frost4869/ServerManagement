using MahApps.Metro.Controls;
using ServerManagement.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ServerManagement.View
{
    /// <summary>
    /// Interaction logic for UpdateAdminPasswordWindow.xaml
    /// </summary>
    public partial class UpdateAdminPasswordWindow : IView
    {
        public static UpdateAdminPasswordWindow Instance
        {
            get;
            private set;
        }

        public UpdateAdminPasswordWindow(AuthenticationViewModel viewModel)
        {
            ViewModel = viewModel;
            InitializeComponent();
            Instance = this;
            Title = "Server Management Tool | Update Admin Password - Version " + Assembly.GetEntryAssembly().GetName().Version;
            this.EnableDWMDropShadow = true;
        }

        #region IView Members
        public IViewModel ViewModel
        {
            get { return DataContext as IViewModel; }
            set { DataContext = value; }
        }
            #endregion
    }


}
