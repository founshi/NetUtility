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
        private static string LogPath = string.Empty;

        private Log()
        { }
        static Log()
        {
            LogPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            LogPath = Path.Combine(LogPath, "Log");
        }


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

        public static void SetLog(string context, System.Windows.Forms.ListBox mlistBox)
        {
            CreateDirectory(LogPath);
            context = "[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]" + context;
            string fileName = Path.Combine(LogPath, DateTime.Now.ToString("yyyyMMdd") + ".log");
            Log.CreateEmptyFile(fileName);
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
            if (mlistBox != null)
            {
                mlistBox.Items.Add(context);
                if (mlistBox.Items.Count >= 100)
                {
                    mlistBox.Items.RemoveAt(0);
                }
            }

        }

        public static void SetLog(string context)
        {
            CreateDirectory(LogPath);
            context = "[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]" + context;
            string fileName = Path.Combine(LogPath, DateTime.Now.ToString("yyyyMMdd") + ".log");
            Log.CreateEmptyFile(fileName);
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

        private static void CreateDirectory(string targetDir)
        {
            DirectoryInfo dir = new DirectoryInfo(targetDir);
            if (!dir.Exists)
                dir.Create();
        }


    }
}
