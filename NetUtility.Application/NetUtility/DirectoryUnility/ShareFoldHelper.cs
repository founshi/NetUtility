using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

namespace NetUtility.DirectoryUnility
{
    /// <summary>
    /// 共享文件夹的读取
    /// </summary>
    public class ShareFoldHelper : IDisposable
    {

        public string Username { get; private set; }
        public string Password { get; private set; }
        public string Ip { get; private set; }
        ShareFlod tool = null;
        private bool disposed;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="ip"></param>
        public ShareFoldHelper(string username, string password, string ip)
        {
            this.Username = username;
            this.Password = password;
            this.Ip = ip;
            tool = new ShareFlod(username, password, ip);
        }
        //\\10.170.1.67\aaftp\AA
        /// <summary>
        /// 获取文件夹下所有文件
        /// </summary>
        /// <param name="foldPath"></param>
        /// <returns></returns>
        public List<FileType> GetAllFile(string foldPath)
        {
            List<FileType> mlist = new List<FileType>();
            try
            {
                string path = @"\\" + this.Ip + @"\" + foldPath;// + "$";
                DirectoryInfo dicInfo = new DirectoryInfo(path);
                FileInfo[] textFiles = dicInfo.GetFiles("*.csv", SearchOption.AllDirectories);//获取所有目录包含子目录下的文件    
                foreach (FileInfo item in textFiles)
                {
                    FileType tmp = new FileType();
                    tmp.FilePath = item.FullName;
                    tmp.FileName = item.Name;
                    tmp.MachieMac = item.Name.Substring(0, 17);
                    mlist.Add(tmp);
                }
                return mlist;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public void RemoveFile(string srcFile, string dstFile)
        {
            if (File.Exists(dstFile)) KillFile(dstFile, 10);
            string dstFold = Path.GetDirectoryName(dstFile);
            if (!Directory.Exists(dstFold))
                CreateDirectory(dstFold);
            File.Move(srcFile, dstFile);
        }
        private void CreateDirectory(string targetDir)
        {
            DirectoryInfo dir = new DirectoryInfo(targetDir);
            if (!dir.Exists)
                dir.Create();
        }
        /// <summary>
        /// 查看文件是否被占用
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns>true表示正在使用,false没有使用  </returns>
        private bool IsFileInUse(string fileName)
        {
            bool inUse = true;
            FileStream fs = null;
            try
            {
                fs = new FileStream(fileName, FileMode.Open, FileAccess.Read,
                FileShare.None);
                inUse = false;
            }
            catch
            {
            }
            finally
            {
                if (fs != null) fs.Close();
            }
            return inUse;//true表示正在使用,false没有使用  
        }

        /// <summary>
        /// 强力粉碎文件，文件如果被打开，很难粉碎
        /// </summary>
        /// <param name="filename">文件全路径</param>
        /// <param name="deleteCount">删除次数</param>
        /// <param name="randomData">随机数据填充文件，默认true</param>
        /// <param name="blanks">空白填充文件，默认false</param>
        /// <returns>true：粉碎成功，false：粉碎失败</returns>
        private bool KillFile(string filename, int deleteCount, bool randomData = true, bool blanks = false)
        {
            const int bufferLength = 1024000;
            bool ret = true;
            try
            {
                using (FileStream stream = new FileStream(filename, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    FileInfo f = new FileInfo(filename);
                    long count = f.Length;
                    long offset = 0;
                    var rowDataBuffer = new byte[bufferLength];
                    while (count >= 0)
                    {
                        int iNumOfDataRead = stream.Read(rowDataBuffer, 0, bufferLength);
                        if (iNumOfDataRead == 0)
                        {
                            break;
                        }
                        if (randomData)
                        {
                            Random randombyte = new Random();
                            randombyte.NextBytes(rowDataBuffer);
                        }
                        else if (blanks)
                        {
                            for (int i = 0; i < iNumOfDataRead; i++)
                                rowDataBuffer[i] = 0;
                        }
                        else
                        {
                            for (int i = 0; i < iNumOfDataRead; i++)
                                rowDataBuffer[i] = Convert.ToByte(Convert.ToChar(deleteCount));
                        }
                        // 写新内容到文件。
                        for (int i = 0; i < deleteCount; i++)
                        {
                            stream.Seek(offset, SeekOrigin.Begin);
                            stream.Write(rowDataBuffer, 0, iNumOfDataRead);
                        }
                        offset += iNumOfDataRead;
                        count -= iNumOfDataRead;
                    }
                }
                //每一个文件名字符代替随机数从0到9。
                string newName = "";
                do
                {
                    Random random = new Random();
                    string cleanName = Path.GetFileName(filename);
                    string dirName = Path.GetDirectoryName(filename);
                    int iMoreRandomLetters = random.Next(9);
                    // 为了更安全，不要只使用原文件名的大小，添加一些随机字母。
                    for (int i = 0; i < cleanName.Length + iMoreRandomLetters; i++)
                    {
                        newName += random.Next(9).ToString();
                    }
                    newName = dirName + "\\" + newName;
                } while (File.Exists(newName));
                // 重命名文件的新的随机的名字。
                File.Move(filename, newName);
                File.Delete(newName);
            }
            catch
            {
                //可能其他原因删除失败了，使用我们自己的方法强制删除
                try
                {
                    string fileName = filename;//要检查被那个进程占用的文件
                    Process tool = new Process { StartInfo = { FileName = "handle.exe", Arguments = fileName + " /accepteula", UseShellExecute = false, RedirectStandardOutput = true } };
                    tool.Start();
                    tool.WaitForExit();
                    string outputTool = tool.StandardOutput.ReadToEnd();
                    string matchPattern = @"(?<=\s+pid:\s+)\b(\d+)\b(?=\s+)";
                    foreach (Match match in Regex.Matches(outputTool, matchPattern))
                    {

                        //结束掉所有正在使用这个文件的程序
                        Process.GetProcessById(int.Parse(match.Value)).Kill();
                    }
                    File.Delete(fileName);
                }
                catch
                {
                    ret = false;
                }
            }
            return ret;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                tool.Dispose();
                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

    }


    public class ShareFlod : IDisposable
    {
        // obtains user token         
        [DllImport("advapi32.dll", SetLastError = true)]
        static extern bool LogonUser(string pszUsername, string pszDomain, string pszPassword,
            int dwLogonType, int dwLogonProvider, ref IntPtr phToken);
        // closes open handes returned by LogonUser         
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        extern static bool CloseHandle(IntPtr handle);

        [DllImport("Advapi32.DLL")]
        static extern bool ImpersonateLoggedOnUser(IntPtr hToken);

        [DllImport("Advapi32.DLL")]
        static extern bool RevertToSelf();
        const int LOGON32_PROVIDER_DEFAULT = 0;
        const int LOGON32_LOGON_NEWCREDENTIALS = 9;//域控中的需要用:Interactive = 2         
        private bool disposed;

        public ShareFlod(string username, string password, string ip)
        {
            // initialize tokens         
            IntPtr pExistingTokenHandle = new IntPtr(0);
            IntPtr pDuplicateTokenHandle = new IntPtr(0);

            try
            {
                // get handle to token         
                bool bImpersonated = LogonUser(username, ip, password,
                    LOGON32_LOGON_NEWCREDENTIALS, LOGON32_PROVIDER_DEFAULT, ref pExistingTokenHandle);

                if (bImpersonated)
                {
                    if (!ImpersonateLoggedOnUser(pExistingTokenHandle))
                    {
                        int nErrorCode = Marshal.GetLastWin32Error();
                        throw new Exception("ImpersonateLoggedOnUser error;Code=" + nErrorCode);
                    }
                }
                else
                {
                    int nErrorCode = Marshal.GetLastWin32Error();
                    throw new Exception("LogonUser error;Code=" + nErrorCode);
                }
            }
            finally
            {
                // close handle(s)         
                if (pExistingTokenHandle != IntPtr.Zero)
                    CloseHandle(pExistingTokenHandle);
                if (pDuplicateTokenHandle != IntPtr.Zero)
                    CloseHandle(pDuplicateTokenHandle);
            }
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                RevertToSelf();
                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

    }

    public class FileType
    {
        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath { get; set; }
        /// <summary>
        /// 产生机台的csv文件
        /// </summary>
        public string MachieMac { get; set; }


    }
}

