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
            LogPath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            LogPath = Path.Combine(LogPath, "Log");
        }
        private static void CreateEmptyFile(string fileName)
        {
            if (File.Exists(fileName)) return;
            FileStream fsteam = File.Create(fileName);
            fsteam.Close();
            fsteam.Dispose();
        }
        public static void SetLog(string context)
        {
            if (string.IsNullOrEmpty(context.Trim())) return;
            CreateDirectory(LogPath);
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
        public static void CreateDirectory(string targetDir)
        {
            DirectoryInfo dir = new DirectoryInfo(targetDir);
            if (!dir.Exists)
                dir.Create();
        }

        public static void SetLog(string context, string filePath)
        {
            if (string.IsNullOrEmpty(context.Trim())) return;
            Log.CreateEmptyFile(filePath);
            StreamWriter sw = null;
            try
            {
                lock (_lock)
                {
                    sw = File.AppendText(filePath);
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

    }
}
