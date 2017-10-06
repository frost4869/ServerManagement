using Microsoft.Win32;
using ServerManagement.ViewModel;
using System;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Excel = Microsoft.Office.Interop.Excel;
namespace ServerManagement.View
{
    /// <summary>
    /// Interaction logic for ImportServers.xaml
    /// </summary>
    public partial class ImportServers : UserControl
    {
        public ImportServers()
        {
            InitializeComponent();
            OpenFileDialog fopen = new OpenFileDialog();
            fopen.Filter = "All Files|*.*|Excel Files|*.xlsx";
            fopen.ShowDialog();

            if (fopen.FileName != "")
            {
                ImportDatagrid.DataContext = new ImportServerViewModel(fopen.FileName);
            }
        }

        private void headerCheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void headerCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void CopyButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {

        }


    }
}
