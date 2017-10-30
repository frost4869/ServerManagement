using AutoUpdaterDotNET;
using ServerManagement.ViewModel;
using System.Globalization;
using System.Reflection;
using System.Windows.Input;

namespace ServerManagement.View
{
    public interface IView
    {
        IViewModel ViewModel
        {
            get;
            set;
        }

        void Show();
    }
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : IView
    {
        public static LoginWindow Instance
        {
            get;
            private set;
        }
        public LoginWindow(AuthenticationViewModel viewModel)
        {
            ViewModel = viewModel;
            InitializeComponent();
            Instance = this;
            Title = "Server Management Tool - Version " + Assembly.GetEntryAssembly().GetName().Version;
            this.EnableDWMDropShadow = true;
            FocusManager.SetFocusedElement(this, txtUsername);

            //AutoUpdater.CurrentCulture = new CultureInfo("en-EN");
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

        private void Hyperlink_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            AutoUpdater.Start("https://raw.githubusercontent.com/frost4869/uploadfiles/master/updateServer.xml");
        }
    }
}
