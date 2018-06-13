using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace NetUtility
{
    public class ByteUnility
    {
        /// <summary>
        /// 图片 转换 byte数组
        /// </summary>
        /// <param name="pic"></param>
        /// <param name="fmt"></param>
        /// <returns></returns>
        public static byte[] Image2ByteArray(Image pic, System.Drawing.Imaging.ImageFormat fmt)
        {
            MemoryStream mem = new MemoryStream();
            pic.Save(mem, fmt);
            mem.Flush();
            return mem.ToArray();
        }
        /// <summary>
        /// byte数组 转换 图片
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static Image ByteArray2Image(byte[] bytes)
        {
            MemoryStream mem = new MemoryStream(bytes, true);
            mem.Read(bytes, 0, bytes.Length);
            mem.Flush();
            Image aa = Image.FromStream(mem);
            return aa;
        }

        /// <summary>
        /// 字符串转换成 字节数组
        /// </summary>
        /// <param name="str"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static byte[] String2ByteArray(string str, Encoding encoding)
        {
            if (encoding == null)
            {
                return Encoding.UTF8.GetBytes(str);
            }
            else
            {
                return encoding.GetBytes(str);
            }
        }
        /// <summary>
        /// byte数组转成字符串
        /// </summary>
        /// <param name="byteArray"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string ByteArray2String(byte[] byteArray, Encoding encoding)
        {
            if (encoding == null) return Encoding.UTF8.GetString(byteArray);
            else return encoding.GetString(byteArray);
        }





    }
}
