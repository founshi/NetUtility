using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace NetUtility
{
    /// <summary>
    /// 产生验证码 这里在生产图片的时候，如果是汉字就有点问题，
    /// 应该先确认字体的大小 和字数 计算出画布的大小 在来画 
    /// 这样应该要好些
    /// </summary>
    public class VerificationCode
    {
        //private static int letterCount = 4;   //验证码位数
        private static int letterWidth = 16;  //单个字体的宽度范围
        private static int letterHeight = 20; //单个字体的高度范围
        private static Font[] fonts = 
    {
       new Font(new FontFamily("Times New Roman"),10 +VerificationCode.GenerateRandomInteger(4,8),System.Drawing.FontStyle.Regular),
       new Font(new FontFamily("Georgia"), 10 + VerificationCode.GenerateRandomInteger(4,8),System.Drawing.FontStyle.Regular),
       new Font(new FontFamily("Arial"), 10 +VerificationCode.GenerateRandomInteger(4,8),System.Drawing.FontStyle.Regular),
       new Font(new FontFamily("Comic Sans MS"), 10 + VerificationCode.GenerateRandomInteger(4,8),System.Drawing.FontStyle.Regular)
    };

        /// <summary>
        /// 产生全数字的验证码
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string VCodeNumber(int length)
        {
            string result = string.Empty;
            for (int i = 0; i < length; i++)
            {
                result += VerificationCode.GenerateRandomInteger(0, 10);
            }
            return result;
        }
        /// <summary>
        /// 产生不重复的随机随
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static int GenerateRandomInteger(int min = 0, int max = int.MaxValue)
        {
            var randomNumberBuffer = new byte[10];
            new RNGCryptoServiceProvider().GetBytes(randomNumberBuffer);
            return new Random(BitConverter.ToInt32(randomNumberBuffer, 0)).Next(min, max);
            //原来，Random是需要一个随机数作为“种子”的，当这个种子相同时，那么产生的随机数也是相同的，
            //有同学肯定会说，我们平时用的时候没有指定“种子”数据，也能产生我想要的随机数啊！ 
            //其实，当我们没有指定“种子”的时候，Random时默认以当前时间作为种子的，当高并发访问的情况下
            //，如果使用时间作为种子数据，这显然就很有可能产生相同的随机数，这显然就不那么“随机”了，
            //所以该方法看似多余的方法都只是为了利用RNGCryptoServiceProvider().GetBytes（）
            //产生一个足够随机的byte[]，然后再把该byte[]转换成数字，那么该数字就能基本不会重复了，
            //也就是”种子”不重复，所以随机数也不会重复了

        }

        /// <summary>
        /// 生产数字加字母的验证码
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string VCodeStringNumber(int length)
        {
            char[] Pattern = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
            string result = string.Empty;
            for (int i = 0; i < length; i++)
            {
                int _index = GenerateRandomInteger(0, Pattern.Length);
                result += Pattern[_index];
            }
            return result;
        }
        /// <summary>
        /// 生产全部都是 字母的验证码
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string VCodeString(int length)
        {
            char[] Pattern = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
            string result = string.Empty;
            for (int i = 0; i < length; i++)
            {
                int _index = GenerateRandomInteger(0, Pattern.Length);
                result += Pattern[_index];

            }
            return result;
        }
        /// <summary>
        /// 创建纯数字的 验证码
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static Bitmap CreateImageByNumber(int length)
        {
            string _Text = VerificationCode.VCodeNumber(length);

            return CreateNewImage(_Text);
        }
        /// <summary>
        /// 创建字母的验证码
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static Bitmap CreateImageByString(int length)
        {
            string _Text = VerificationCode.VCodeString(length);

            return CreateNewImage(_Text);
        }
        /// <summary>
        /// 创建数字加字母的验证码
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static Bitmap CreateImage(int length)
        {
            string _Text = VerificationCode.VCodeStringNumber(length);

            return CreateNewImage(_Text);
        }



        /// <summary>
        /// 字体随机颜色
        /// </summary>
        public static Color GetRandomColor()
        {
            Random RandomNum_First = new Random((int)DateTime.Now.Ticks);
            System.Threading.Thread.Sleep(RandomNum_First.Next(50));
            Random RandomNum_Sencond = new Random((int)DateTime.Now.Ticks);
            int int_Red = RandomNum_First.Next(180);
            int int_Green = RandomNum_Sencond.Next(180);
            int int_Blue = (int_Red + int_Green > 300) ? 0 : 400 - int_Red - int_Green;
            int_Blue = (int_Blue > 255) ? 255 : int_Blue;
            return Color.FromArgb(int_Red, int_Green, int_Blue);
        }
        /// <summary>
        /// 正弦曲线Wave扭曲图片
        /// </summary>
        /// <param name="srcBmp">图片路径</param>
        /// <param name="bXDir">如果扭曲则选择为True</param>
        /// <param name="nMultValue">波形的幅度倍数，越大扭曲的程度越高,一般为3</param>
        /// <param name="dPhase">波形的起始相位,取值区间[0-2*PI)</param>
        public static System.Drawing.Bitmap TwistImage(Bitmap srcBmp, bool bXDir, double dMultValue, double dPhase)
        {
            double PI = 6.283185307179586476925286766559;
            Bitmap destBmp = new Bitmap(srcBmp.Width, srcBmp.Height);
            Graphics graph = Graphics.FromImage(destBmp);
            graph.FillRectangle(new SolidBrush(Color.White), 0, 0, destBmp.Width, destBmp.Height);
            graph.Dispose();
            double dBaseAxisLen = bXDir ? (double)destBmp.Height : (double)destBmp.Width;
            for (int i = 0; i < destBmp.Width; i++)
            {
                for (int j = 0; j < destBmp.Height; j++)
                {
                    double dx = 0;
                    dx = bXDir ? (PI * (double)j) / dBaseAxisLen : (PI * (double)i) / dBaseAxisLen;
                    dx += dPhase;
                    double dy = Math.Sin(dx);
                    int nOldX = 0, nOldY = 0;
                    nOldX = bXDir ? i + (int)(dy * dMultValue) : i;
                    nOldY = bXDir ? j : j + (int)(dy * dMultValue);

                    Color color = srcBmp.GetPixel(i, j);
                    if (nOldX >= 0 && nOldX < destBmp.Width
                     && nOldY >= 0 && nOldY < destBmp.Height)
                    {
                        destBmp.SetPixel(nOldX, nOldY, color);
                    }
                }
            }
            srcBmp.Dispose();
            return destBmp;
        }
        /// <summary>
        /// 根据字符串产生验证码
        /// </summary>
        /// <param name="_Text"></param>
        /// <returns></returns>
        public static System.Drawing.Bitmap CreateNewImage(string _Text)
        {
            int int_ImageWidth = _Text.Length * letterWidth;
            Bitmap image = new Bitmap(int_ImageWidth, letterHeight);
            Graphics g = Graphics.FromImage(image);
            g.Clear(Color.White);

            for (int i = 0; i < 2; i++)
            {
                int x1 = VerificationCode.GenerateRandomInteger(0, image.Width - 1);
                int x2 = VerificationCode.GenerateRandomInteger(0, image.Width - 1);
                int y1 = VerificationCode.GenerateRandomInteger(0, image.Height - 1);
                int y2 = VerificationCode.GenerateRandomInteger(0, image.Height - 1);
                g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
            }

            int _x = -12, _y = 0;
            for (int int_index = 0; int_index < _Text.Length; int_index++)
            {
                _x += VerificationCode.GenerateRandomInteger(12, 16);
                _y = VerificationCode.GenerateRandomInteger(-2, 2);
                string str_char = _Text.Substring(int_index, 1);
                str_char = VerificationCode.GenerateRandomInteger(0, 1) == 1 ? str_char.ToLower() : str_char.ToUpper();
                Brush newBrush = new SolidBrush(GetRandomColor());
                Point thePos = new Point(_x, _y);
                g.DrawString(str_char, fonts[VerificationCode.GenerateRandomInteger(0, fonts.Length - 1)], newBrush, thePos);
            }
            for (int i = 0; i < 10; i++)
            {
                int x = VerificationCode.GenerateRandomInteger(0, image.Width - 1);
                int y = VerificationCode.GenerateRandomInteger(0, image.Height - 1);
                image.SetPixel(x, y, Color.FromArgb(VerificationCode.GenerateRandomInteger(0, 255), VerificationCode.GenerateRandomInteger(0, 255), VerificationCode.GenerateRandomInteger(0, 255)));
            }
            image = TwistImage(image, true, VerificationCode.GenerateRandomInteger(1, 3), VerificationCode.GenerateRandomInteger(4, 6));
            g.DrawRectangle(new Pen(Color.LightGray, 1), 0, 0, int_ImageWidth - 1, (letterHeight - 1));


            return image;
        }


    }
}
