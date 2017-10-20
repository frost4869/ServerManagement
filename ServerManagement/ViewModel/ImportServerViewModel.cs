using System;
using System.ComponentModel;
using System.Data;
using System.Reflection;
using Excel = Microsoft.Office.Interop.Excel;
using System.Threading.Tasks;
using MahApps.Metro.Controls;
using System.Windows.Controls;
using System.Windows;
using System.Linq;
using MahApps.Metro.Controls.Dialogs;

namespace ServerManagement.ViewModel
{
    class ImportServerViewModel : INotifyPropertyChanged
    {
        private DataView _data;
        private string _fileName;
        private StackPanel _progressSection;

        public ImportServerViewModel(string FileName, StackPanel ProgressSection)
        {
            _fileName = FileName;
            _progressSection = ProgressSection;
            LoadImportedDataAsync();
        }

        public DataView Data
        {
            get
            {
                return _data;
            }
            set
            {
                _data = value;
                NotifyPropertyChanged("Data");
            }
        }

        public async Task LoadImportedDataAsync()
        {
            var metroWindow = (System.Windows.Application.Current.Windows.OfType<Window>().SingleOrDefault(q => q.IsActive) as MetroWindow);

            await Task.Run(() =>
            {
                Excel.Application app = new Excel.Application();
                Excel.Workbook wb = app.Workbooks.Open(_fileName);

                try
                {
                    Excel._Worksheet sheet = wb.Sheets[1];
                    Excel.Range range = sheet.UsedRange;

                    int rows = range.Rows.Count;
                    int cols = range.Columns.Count;

                    DataTable dt = new DataTable();

                    for (int i = 1; i <= cols; i++)
                    {
                        dt.Columns.Add(range.Cells[1, i].Value.ToString());
                    }
                    for (int i = 2; i <= rows; i++)
                    {
                        DataRow dr = dt.NewRow();
                        for (int j = 1; j <= cols; j++)
                        {
                            if (range.Cells[i, j].Value != null && range.Cells[i, j] != null)
                                dr[j - 1] = range.Cells[i, j].Value.ToString();
                        }
                        dt.Rows.Add(dr);
                        dt.AcceptChanges();
                    }
                    wb.Close(true, Missing.Value, Missing.Value);

                    Data = dt.DefaultView;
                    _progressSection.Dispatcher.Invoke(new Action(() =>
                    {
                        _progressSection.Visibility = System.Windows.Visibility.Collapsed;
                    }));
                }
                catch (Exception ex)
                {
                    metroWindow.ShowMessageAsync("Error", ex.Message);
                }
                finally
                {
                    app.Quit();
                    wb = null;
                }
            });
        }

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
