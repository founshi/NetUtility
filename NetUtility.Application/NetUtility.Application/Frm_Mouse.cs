using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NetUtility.App
{
    public partial class Frm_Mouse : Form
    {
        Point _StartPoint = new Point();
        Point _EndPoint = new Point();
        bool CanDraw = false;
        bool OneDraw = false;
        public Frm_Mouse()
        {
            InitializeComponent();
        }

        private void Frm_Mouse_Load(object sender, EventArgs e)
        {

        }


        /// <summary>
        /// 根据坐标点获取屏幕图像
        /// </summary>
        /// <param name="x1">左上角横坐标</param>
        /// <param name="y1">左上角纵坐标</param>
        /// <param name="x2">右下角横坐标</param>
        /// <param name="y2">右下角纵坐标</param>
        /// <returns></returns>
        public static Bitmap GetScreen(int x1, int y1, int x2, int y2)
        {
            int w = (x2 - x1);
            int h = (y2 - y1);
            if (w <= 0) return null;
            if (h <= 0) return null;

            Bitmap myImage = new Bitmap(w, h);
            Graphics g = Graphics.FromImage(myImage);
            g.CopyFromScreen(new Point(x1, y1), new Point(0, 0), new Size(w, h));
            IntPtr dc1 = g.GetHdc();
            g.ReleaseHdc(dc1);
            return myImage;
        }

        private void Frm_Mouse_MouseDown(object sender, MouseEventArgs e)
        {
            CanDraw = false;
            OneDraw = true;//一次绘制开始
            _StartPoint.X = e.X;
            _StartPoint.Y = e.Y;
        }

        private void Frm_Mouse_MouseUp(object sender, MouseEventArgs e)
        {
            _EndPoint.X = e.X;
            _EndPoint.Y = e.Y;


            CanDraw = true;
            this.Invalidate();
            //Bitmap _Bitmap = GetScreen(_StartPoint.X, _StartPoint.Y, _EndPoint.X, _EndPoint.Y);
            //if (null == _Bitmap) return;
            //string _savePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\123.bmp";
            //_Bitmap.Save(_savePath, ImageFormat.Bmp);
            OneDraw = false;//一次绘制结束
        }

        private void Frm_Mouse_Paint(object sender, PaintEventArgs e)
        {
            if ((CanDraw) && (OneDraw))
            {
                CanDraw = false;
                float _height = Math.Abs(_EndPoint.Y - _StartPoint.Y);
                float _width = Math.Abs(_EndPoint.X - _StartPoint.X);
                if (_EndPoint.Y >= _StartPoint.Y)
                {
                    e.Graphics.DrawRectangle(Pens.Red, _StartPoint.X, _StartPoint.Y, _width, _height);
                }
                else
                {
                    e.Graphics.DrawRectangle(Pens.Red, _EndPoint.X, _EndPoint.Y, _width, _height);
                }
            }
        }

      

        private void Frm_Mouse_MouseMove(object sender, MouseEventArgs e)
        {
            if (OneDraw)
            {
                CanDraw = false;
                _EndPoint.X = e.X;
                _EndPoint.Y = e.Y;
                CanDraw = true;
                this.Invalidate();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            this.pictureBox1.Image = VerificationCode.CreateNewImage("我是释放");
        }

    }
}
