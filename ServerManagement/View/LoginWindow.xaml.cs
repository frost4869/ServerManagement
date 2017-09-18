using MahApps.Metro.Controls;
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
using System.Windows.Shapes;

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
