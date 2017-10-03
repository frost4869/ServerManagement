using ServerManagement.Model;
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
    /// Interaction logic for IpMac.xaml
    /// </summary>
    public partial class IpMac : UserControl
    {
        public IpMac()
        {
            InitializeComponent();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            IpModel removedIp = ((FrameworkElement)sender).DataContext as IpModel;
            if(removedIp != null)
            {
                removedIp.IsRemoved = true;
            }
            ((StackPanel)Parent).Children.Remove(this);
        }
    }
}
