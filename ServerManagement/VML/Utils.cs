using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace ServerManagement.VML
{
    public static class Utils
    {
        public static string GetEntityConnectionString()
        {
            SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder();

            // Set the properties for the data source.

            sqlBuilder.InitialCatalog = "ServerManagement";
            sqlBuilder.PersistSecurityInfo = true;
            sqlBuilder.MultipleActiveResultSets = true;
            sqlBuilder.ApplicationName = "EntityFramework";
            sqlBuilder.UserID = "user_conn_server";
            sqlBuilder.Password = "Si*2bjSu#B";
            sqlBuilder.DataSource = "11.0.4.221";
            //sqlBuilder.UserID = "sa";
            //sqlBuilder.Password = "aptx4869";
            //sqlBuilder.DataSource = "ANM-SONNVH";

            // Build the SqlConnection connection string.
            string providerString = sqlBuilder.ToString();

            EntityConnectionStringBuilder entityConStrBuilder = new EntityConnectionStringBuilder();
            entityConStrBuilder.Provider = "System.Data.SqlClient";
            entityConStrBuilder.ProviderConnectionString = providerString;
            entityConStrBuilder.Metadata = @"res://*/Model.Entity.ServerManagementModel.csdl|res://*/Model.Entity.ServerManagementModel.ssdl|res://*/Model.Entity.ServerManagementModel.msl";

            return entityConStrBuilder.ToString();
        }

        public static void WriteLog(string message)
        {
            var path = AppDomain.CurrentDomain.BaseDirectory + "Log";
            Directory.CreateDirectory(path);

            using (StreamWriter sw = File.AppendText(path + "/Log - " + DateTime.Now.ToShortDateString().Replace("/", ".") + ".txt"))
            {
                sw.Write("\r\nLog Entry : ");
                sw.WriteLine("{0}", DateTime.Now.ToLongTimeString());
                sw.WriteLine("  :");
                sw.WriteLine("  =====>:{0}", message);
                sw.WriteLine("-------------------------------");
            }
        }

        public static async Task<MessageDialogResult> ShowMessageBoxAsync(string title, string mess)
        {
            var metroWindow = (Application.Current.MainWindow as MetroWindow);
            return await metroWindow.ShowMessageAsync(title, mess);
        }

        public static T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            //get parent item
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);

            //we've reached the end of the tree
            if (parentObject == null) return null;

            //check if the parent matches the type we're looking for
            T parent = parentObject as T;
            if (parent != null)
                return parent;
            else
                return FindParent<T>(parentObject);
        }

        public static async Task<DataTable> ToDataTable<T>(this IList<T> data)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable dt = new DataTable();
            foreach (PropertyDescriptor prop in properties)
            {
                dt.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }
            foreach (T item in data)
            {
                DataRow row = dt.NewRow();
                foreach (PropertyDescriptor pdt in properties)
                {
                    row[pdt.Name] = pdt.GetValue(item) ?? DBNull.Value;
                }
                dt.Rows.Add(row);
            }
            return dt;
        }
    }
}
