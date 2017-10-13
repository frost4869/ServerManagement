using AutoUpdaterDotNET;
using ServerManagement.View;
using ServerManagement.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Security.Permissions;
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

namespace ServerManagement
{
    /// <summary>
    /// Interaction logic for ReaderWindow.xaml
    /// </summary>
    [PrincipalPermission(SecurityAction.Demand, Role = "Reader")]
    public partial class ReaderWindow : IView
    {
        public static ReaderWindow Instance
        {
            get;
            private set;
        }
        public ReaderWindow(AuthenticationViewModel viewModel)
        {
            ViewModel = viewModel;
            InitializeComponent();
            Instance = this;
            this.EnableDWMDropShadow = true;
            WindowState = WindowState.Maximized;
            WindowStyle = WindowStyle.SingleBorderWindow;
            Title = "Server Management Tool - Version " + Assembly.GetEntryAssembly().GetName().Version;

            AutoUpdater.CurrentCulture = new CultureInfo("en-EN");
            AutoUpdater.LetUserSelectRemindLater = true;
            AutoUpdater.RemindLaterTimeSpan = RemindLaterFormat.Minutes;
            AutoUpdater.RemindLaterAt = 1;
            AutoUpdater.ReportErrors = true;
        }

        #region IView Members
        public IViewModel ViewModel
        {
            get { return DataContext as IViewModel; }
            set { DataContext = value; }
        }
        #endregion

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            AutoUpdater.Start("https://raw.githubusercontent.com/frost4869/uploadfiles/master/updateServer.xml");
        }
    }
}
