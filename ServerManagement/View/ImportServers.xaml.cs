using AutoMapper;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;
using ServerManagement.Model;
using ServerManagement.Model.Entity;
using ServerManagement.ViewModel;
using ServerManagement.VML;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
                                    Active = true,
                                };
                                db.Servers.Add(server);
                                db.SaveChanges();

                                for (int i = 5; i <= 8; i++)
                                {
                                    if (!string.IsNullOrEmpty(items[i]))
                                        db.IPs.Add(new IP
                                        {
                                            IPAddress = items[i],
                                            ServerId = server.Id,
                                            Active = true
                                        });
                                }
                                db.SaveChanges();
                                progress = (progress * 100) / ImportDatagrid.Items.Count;
                            }
                            trans.Commit();
                            await controller.CloseAsync();
                            await metroWindow.ShowMessageAsync("Success", "Saved");
                        }
                        catch (Exception ex)
                        {
                            await controller.CloseAsync();
                            await metroWindow.ShowMessageAsync("Error", ex.Message);
                            trans.Rollback();
                        }
                    }
                }
            }
        }
    }
}
