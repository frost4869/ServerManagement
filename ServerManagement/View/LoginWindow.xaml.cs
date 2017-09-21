using ServerManagement.ViewModel;
using System.Reflection;

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
