using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace NetUtility.App
{
    /// <summary>
    /// 移动无边框的窗体
    /// </summary>
    public partial class FRM_NoFromBody : Form
    {
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int IParam);
        public const int WM_SYSCOMMAND = 0x0112;
        public const int SC_MOVE = 0xF010;
        public const int HTCAPTION = 0x0002;
        [DllImport("user32")]
        private static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, IntPtr lParam);
        private const int WM_SETREDRAW = 0xB;
        public FRM_NoFromBody()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FRM_NoFromBody_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.Tag != null && ((int[])this.Tag)[0] == Control.MousePosition.X && ((int[])this.Tag)[1] == Control.MousePosition.Y)
            {

                if (this.WindowState != FormWindowState.Maximized)
                {
                    this.Opacity = 0;
                    this.Tag = null;
                    this.WindowState = FormWindowState.Maximized;
                }
                else
                {
                    this.Tag = null;
                    this.WindowState = FormWindowState.Normal;
                    //InvalidateRect(IntPtr.Zero, IntPtr.Zero, true);
                }

            }
            else
            {
                this.Tag = new int[2] { Control.MousePosition.X, Control.MousePosition.Y };
            }
            //刷新桌面
            if (this.WindowState == FormWindowState.Normal)
            {
                ReleaseCapture();
                SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
            }
        }

      
        /// <summary>  
        /// GMT时间转成本地时间  
        /// </summary>  
        /// <param name="gmt">字符串形式的GMT时间</param>  
        /// <returns></returns> 
        public static DateTime GMT2Local(string gmt)
        {
            DateTime dt = DateTime.MinValue;
            try
            {
                string pattern = "";
                if (gmt.IndexOf("+0") != -1)
                {
                    gmt = gmt.Replace("GMT", "");
                    pattern = "ddd, dd-MMM-yyyy HH':'mm':'ss zzz";
                }
                if (gmt.ToUpper().IndexOf("GMT") != -1)
                {
                    pattern = "ddd, dd-MMM-yyyy HH':'mm':'ss 'GMT'";
                }
                if (pattern != "")
                {
                    dt = DateTime.ParseExact(gmt, pattern, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.AdjustToUniversal);
                    dt = dt.ToLocalTime();
                }
                else
                {
                    dt = Convert.ToDateTime(gmt);
                }
            }
            catch
            {
            }
            return dt;
        }
    }
}
