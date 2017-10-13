using AutoUpdaterDotNET;
using MahApps.Metro.Controls;
using ServerManagement.Identity;
using ServerManagement.View;
using ServerManagement.ViewModel;
using System.Globalization;
using System.Reflection;
using System.Security.Permissions;
using System.Windows;
namespace ServerManagement
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [PrincipalPermission(SecurityAction.Demand, Role = "Admin")]
    [PrincipalPermission(SecurityAction.Demand, Role = "Writer")]
    public partial class MainWindow : IView
    {
        #region IView Members
        public IViewModel ViewModel
        {
            get { return DataContext as IViewModel; }
            set { DataContext = value; }
        }
        #endregion
        public static MainWindow Instance
        {
            get;
            private set;
        }
        public MainWindow(AuthenticationViewModel viewModel)
        {
            ViewModel = viewModel;
            InitializeComponent();
            Instance = this;
            this.EnableDWMDropShadow = true;
            WindowState = WindowState.Maximized;
            WindowStyle = WindowStyle.SingleBorderWindow;
            Title = "Server Management Tool - Version " + Assembly.GetEntryAssembly().GetName().Version;

            MetroTabItem item = new MetroTabItem
            {
                Header = "Server List",
            };

            Server view = new Server();
            item.Content = view;
            MainTabControl.Items.Add(item);

            AutoUpdater.CurrentCulture = new CultureInfo("en-EN");
            AutoUpdater.LetUserSelectRemindLater = true;
            AutoUpdater.RemindLaterTimeSpan = RemindLaterFormat.Minutes;
            AutoUpdater.RemindLaterAt = 1;
            AutoUpdater.ReportErrors = true;
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

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            AuthenticationViewModel viewModel = new AuthenticationViewModel(new AuthenticationService());
            RegisterWindow registerWindow = new RegisterWindow(viewModel);
            registerWindow.Show();
        }

        private void Accounts_Click(object sender, RoutedEventArgs e)
        {
            MetroTabItem item = new MetroTabItem
            {
                Header = "Manage Accounts",
                CloseButtonEnabled = true,
            };

            AuthenticationViewModel viewModel = new AuthenticationViewModel();
            Account accounts = new Account(viewModel);
            item.Content = accounts;

            MainTabControl.Items.Add(item);
            item.Focus();
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            AutoUpdater.Start("https://raw.githubusercontent.com/frost4869/uploadfiles/master/updateServer.xml");
        }

        private void Import_Click(object sender, RoutedEventArgs e)
        {
            MetroTabItem item = new MetroTabItem
            {
                Header = "Import Server",
                CloseButtonEnabled = true,
            };

            ImportServers view = new ImportServers();
            item.Content = view;

            MainTabControl.Items.Add(item);
            item.Focus();
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ChangePassword_Click(object sender, RoutedEventArgs e)
        {
            AuthenticationViewModel viewModel = new AuthenticationViewModel(new AuthenticationService());
            UpdateAdminPasswordWindow window = new UpdateAdminPasswordWindow(viewModel);
            window.Show();
        }
    }
}
