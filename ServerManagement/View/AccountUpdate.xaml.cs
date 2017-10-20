using ServerManagement.Model.Entity;
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
    /// Interaction logic for AccountUpdate.xaml
    /// </summary>
    public partial class AccountUpdate : UserControl
    {
        public AccountUpdate()
        {
            InitializeComponent();
            roleComboBox.ItemsSource = Enum.GetValues(typeof(Enums)).Cast<Enums>();
        }
    }
}
