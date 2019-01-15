using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NetUtility
{
    public class FileDialogHelper
    {
        private static string ExcelFilter = "Excel07(*.xlsx)|*.xlsx|Excel03(*.xls)|*.xls";
        public static string Open(string title, string filter, string filename)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = filter;
            dialog.Title = title;
            dialog.RestoreDirectory = true;
            dialog.FileName = filename;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                return dialog.FileName;
            }
            else
            {
                return string.Empty;
            }
        }
        public static string OpenExcel()
        {
            return Open("Excel选择", ExcelFilter);
        }
        public static string Open(string title, string filter)
        {
            return Open(title, filter, string.Empty);
        }
    }
}
