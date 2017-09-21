using MahApps.Metro.Controls;
using ServerManagement.ViewModel;
using System.Reflection;
using System;
using System.Linq;

namespace ServerManagement.View
{
    /// <summary>
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : IView
    {
        public static RegisterWindow Instance
        {
            get;
            private set;
        }

        public RegisterWindow(AuthenticationViewModel viewModel)
        {
            ViewModel = viewModel;
            InitializeComponent();
            Instance = this;
            Title = "Server Management Tool | Register - Version " + Assembly.GetEntryAssembly().GetName().Version;
            this.EnableDWMDropShadow = true;
            txtUsername.Focus();
            roleComboBox.ItemsSource = Enum.GetValues(typeof(RoleEnum)).Cast<RoleEnum>();
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
