using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Windows.Forms;

namespace NetUtility.App
{
    public partial class Frm_Image : Form
    {
        public Frm_Image()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Bitmap _Bitmap = StaticImage.GetImgDesk();
            string _savePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)+@"\123.bmp";
            _Bitmap.Save(_savePath, ImageFormat.Bmp);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Bitmap _Bitmap = StaticImage.FullScreenShot();
            string _savePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\124.bmp";
            _Bitmap.Save(_savePath, ImageFormat.Bmp);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //需要在 属性==>安全性==>启用ClickOne安全设置，这是一个完全可信的应用程序。
            //然后再取消这个选项
            //修改app.manifest文件中requestedExecutionLevel的属性
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            if (principal.IsInRole(WindowsBuiltInRole.Administrator))
            {

            }
            else
            {
                MessageBox.Show("请以管理员身份运行!");
                return;
            }
        }
    }

    static class StaticImage
    {
        /// <summary>
        /// 屏幕截图
        /// </summary>
        /// <returns></returns>
        public static Bitmap GetImgDesk()
        {
            Rectangle rect = System.Windows.Forms.SystemInformation.VirtualScreen;
            //获取屏幕分辨率
            int x_ = rect.Width;
            int y_ = rect.Height;
            //截屏
            Bitmap img = new Bitmap(x_, y_);
            Graphics g = Graphics.FromImage(img);
            g.CopyFromScreen(new Point(0, 0), new Point(0, 0), new Size(x_, y_));
            return img;
        }
        /// <summary>
        /// 全屏截图
        /// </summary>
        /// <returns></returns>
        public static Bitmap FullScreenShot()
        {
            Bitmap image = new Bitmap(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width, System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height);
            using (Graphics g = Graphics.FromImage(image))
            {
                g.CopyFromScreen(new Point(0, 0), new Point(0, 0), System.Windows.Forms.Screen.PrimaryScreen.Bounds.Size);
            }
            return image;
        }
    }


}
