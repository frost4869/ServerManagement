using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ServerManagement.VML
{
    public static class Utils
    {
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
    }
}
