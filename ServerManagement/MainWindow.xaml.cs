using AutoUpdaterDotNET;
using MahApps.Metro.Controls;
using Microsoft.Win32;
using ServerManagement.Identity;
using ServerManagement.Model;
using ServerManagement.View;
using ServerManagement.ViewModel;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Reflection;
using System.Security.Permissions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Excel = Microsoft.Office.Interop.Excel;
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
            SaveFileDialog fsave = new SaveFileDialog();
            fsave.Filter = "Excel File|*.xlsx";
            fsave.ShowDialog();
            if (fsave.FileName != "")
            {
                Export(fsave.FileName);
            }
        }

        private async Task Export(string fileName)
        {
            progressText.Visibility = Visibility.Visible;
            ExportPB.Visibility = Visibility.Visible;

            Excel.Application app = new Excel.Application();
            Excel.Workbook wb = app.Workbooks.Add(Type.Missing);
            Excel._Worksheet sheet = null;
            sheet = wb.ActiveSheet;
            sheet.Name = "Danh sách Servers";

            try
            {
                ServerViewModel viewModel = Server.Instance.DataContext as ServerViewModel;
                ObservableCollection<ServerModel> model = viewModel.Servers;

                DataTable dt = await VML.Utils.ToDataTable(model);
                await Task.Run(() =>
                {

                        //headers
                        for (int i = 1; i <= 14; i++)
                    {
                        sheet.Cells[1, i] = dt.Columns[i - 1].ColumnName;
                        sheet.Cells[1, i].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                        sheet.Cells[1, i].VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                        sheet.Cells[1, i].Font.Size = 11;
                        sheet.Cells[1, i].Font.Bold = true;
                        sheet.Cells[1, i].Borders.Weight = Excel.XlBorderWeight.xlThin;
                        sheet.Cells[1, i].Interior.Color = Excel.XlRgbColor.rgbLightBlue;
                        sheet.Cells[1, i].EntireColumn.AutoFit();
                    }
                        //data
                        int rowCnt = 2;
                    foreach (DataRow row in dt.Rows)
                    {
                        for (int i = 1; i <= 14; i++)
                        {
                            sheet.Cells[rowCnt, i] = row[i - 1].ToString();
                            sheet.Cells[rowCnt, i].HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                            sheet.Cells[rowCnt, i].VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                            sheet.Cells[rowCnt, i].Font.Size = 11;
                            sheet.Cells[rowCnt, i].Borders.Weight = Excel.XlBorderWeight.xlThin;
                            sheet.Cells[rowCnt, i].EntireColumn.AutoFit();
                        }
                        ExportPB.Dispatcher.Invoke(new Action(() =>
                        {
                            ExportPB.Value = rowCnt * 100 / dt.Rows.Count;
                        }));
                        rowCnt++;
                    }
                });

                ExportPB.Visibility = Visibility.Collapsed;
                progressText.Visibility = Visibility.Collapsed;
                wb.SaveAs(fileName);

                MessageBox.Show("Excel file exported at: \n" + fileName, "Finish", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error has occured: \n" + ex.Message, "Error", MessageBoxButton.OK);
                ExportPB.Visibility = Visibility.Collapsed;
                progressText.Visibility = Visibility.Collapsed;
            }
            finally
            {
                app.Quit();
                wb = null;
            }
        }

        private void ChangePassword_Click(object sender, RoutedEventArgs e)
        {
            AuthenticationViewModel viewModel = new AuthenticationViewModel(new AuthenticationService());
            UpdateAdminPasswordWindow window = new UpdateAdminPasswordWindow(viewModel);
            window.Show();
        }
    }
}
