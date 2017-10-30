using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;
using ServerManagement.Model.Entity;
using ServerManagement.ViewModel;
using ServerManagement.VML;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
                ImportDatagrid.DataContext = new ImportServerViewModel(fopen.FileName, ProgressSection);
            }
        }

        private void headerCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            IEnumerable<CheckBox> chkBoxs = this.FindChildren<CheckBox>(true);
            foreach (CheckBox item in chkBoxs)
            {
                item.IsChecked = true;
            }
        }

        private void headerCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            IEnumerable<CheckBox> chkBoxs = this.FindChildren<CheckBox>(true);
            foreach (CheckBox item in chkBoxs)
            {
                item.IsChecked = false;
            }
            btnRemove.Visibility = Visibility.Collapsed;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            btnRemove.Visibility = Visibility.Visible;
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            IEnumerable<CheckBox> chkBoxs = this.FindChildren<CheckBox>(true);
            if (!chkBoxs.Any(q => q.IsChecked.Value))
            {
                btnRemove.Visibility = Visibility.Collapsed;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            SaveImportedData();
        }

        private async Task SaveImportedData()
        {
            var metroWindow = (Application.Current.Windows.OfType<Window>().SingleOrDefault(q => q.IsActive) as MetroWindow);
            MessageDialogStyle style = MessageDialogStyle.AffirmativeAndNegative;
            var result = await metroWindow.ShowMessageAsync("Save", "Save Imported data from Excel file ?", style);

            if (result == MessageDialogResult.Affirmative)
            {
                var controller = await metroWindow.ShowProgressAsync("Saving", "Please wait...");
                controller.SetIndeterminate();
                double progress = 0;
                using (ServerManagementEntities db = new ServerManagementEntities())
                {
                    using (var trans = db.Database.BeginTransaction())
                    {
                        int rowCnt = 1;

                        try
                        {
                            Crypto c = new Crypto(Crypto.CryptoTypes.encTypeTripleDES);
                            foreach (DataRowView row in ImportDatagrid.Items)//each row
                            {
                                controller.SetProgress(progress);
                                string[] items = row.Row.ItemArray
                                    .Select(q => (q == DBNull.Value) ? string.Empty : q)
                                    .Cast<string>()
                                    .ToArray();

                                Model.Entity.Server server = new Model.Entity.Server
                                {
                                    Name = items[0],
                                    HostName = items[1],
                                    Username = items[2],
                                    Password = c.Encrypt(items[3]),
                                    Project = items[4],
                                    NameTag = items[5],
                                    Active = true,
                                };
                                db.Servers.Add(server);
                                db.SaveChanges();

                                for (int i = 6; i <= 9; i++)
                                {
                                    if (!string.IsNullOrEmpty(items[i]))
                                        db.IPs.Add(new IP
                                        {
                                            IPAddress = items[i],
                                            ServerId = server.Id,
                                            Type = i - 5,
                                            Active = true
                                        });
                                }
                                db.SaveChanges();

                                rowCnt++;
                            }
                            trans.Commit();
                            await controller.CloseAsync();
                            await metroWindow.ShowMessageAsync("Success", "Saved");
                        }
                        catch (Exception ex)
                        {
                            await controller.CloseAsync();
                            await metroWindow.ShowMessageAsync("Error", "There is an error with data record at row number: " + rowCnt + "\n\n"
                                                                    + ex.ToString());
                            trans.Rollback();
                        }
                    }
                }
            }
        }

        private void btnRemove_Clicked(object sender, RoutedEventArgs e)
        {
            try
            {
                List<CheckBox> chkBoxs = this.FindChildren<CheckBox>(true)
                    .Where(q => q.Name == "chkBox" && q.IsChecked.Value)
                    .ToList();
                var dataList = ImportDatagrid.ItemsSource as DataView;

                foreach (CheckBox chkBox in chkBoxs)
                {
                    DataGridRow row = VisualTreeHelpers.FindAncestor<DataGridRow>(chkBox);
                    dataList.Delete(row.GetIndex());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK);
            }
        }
    }
}
