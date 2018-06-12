using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NetUtility.FileUnility
{
    public class Log
    {
        private static object _lock = new object();
        /// <summary>
        /// 将字符串追加到指定的文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="context"></param>
        /// <param name="encoding"></param>
        public static void SetLog(string fileName, string context, Encoding encoding)
        {
            if (null == encoding) encoding = Encoding.UTF8;
            if (!Log.FileExist(fileName))
            {
                Log.CreateEmptyFile(fileName);
            }

            StreamWriter sw = null;
            try
            {
                lock (_lock)
                {
                    sw = File.AppendText(fileName);
                    sw.WriteLine(context);
                }
            }
            catch
            { }
            finally
            {
                if (sw != null)
                {
                    sw.Flush();
                    sw.Close();
                    sw.Dispose();
                }
            }


        }
        /// 创建空文件
        /// </summary>
        /// <param name="fileName"></param>
        public static void CreateEmptyFile(string fileName)
        {
            if (Log.FileExist(fileName)) throw new Exception("文件已经存在,不可在创建....");
            FileStream fsteam = File.Create(fileName);
            fsteam.Close();
            fsteam.Dispose();

        }
        /// <summary>
        /// 判断文件是否存在
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool FileExist(string fileName)
        {
            return File.Exists(fileName);
        }

    }
}
