using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

namespace ServerManagement.ViewModel
{
    class ImportServerViewModel : INotifyPropertyChanged
    {
        private DataView _data;
        private string _fileName;

        public ImportServerViewModel(string FileName)
        {
            _fileName = FileName;
        }

        public DataView Data
        {
            get
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

                    return dt.DefaultView;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
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
