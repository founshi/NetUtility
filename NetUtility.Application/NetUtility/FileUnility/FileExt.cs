using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace NetUtility.FileUnility
{
    public class FileExt
    {
        #region 判断两个文件内如是否一致
        /// <summary>
        /// 判断两个文件内如是否一致
        /// </summary>
        /// <param name="filePath1"></param>
        /// <param name="filePath2"></param>
        /// <returns></returns>
        public bool isValidFileContent(string filePath1, string filePath2)
        {
            try
            {
                //创建一个哈希算法对象
                using (HashAlgorithm hash = HashAlgorithm.Create())
                {
                    using (FileStream file1 = new FileStream(filePath1, FileMode.Open), file2 = new FileStream(filePath2, FileMode.Open))
                    {
                        byte[] hashByte1 = hash.ComputeHash(file1);//哈希算法根据文本得到哈希码的字节数组
                        byte[] hashByte2 = hash.ComputeHash(file2);
                        string str1 = BitConverter.ToString(hashByte1);//将字节数组装换为字符串
                        string str2 = BitConverter.ToString(hashByte2);
                        return (str1 == str2);//比较哈希码
                    }
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

        }
        #endregion
        #region 大文件的拷贝
        /// <summary>
        /// 大文件的拷贝
        /// </summary>
        /// <param name="srcFilePath">源文件</param>
        /// <param name="dstFilePath">目标文件</param>
        public static void CopyBigFile(string srcFilePath, string dstFilePath)
        {
            System.IO.FileStream streamReader = new System.IO.FileStream(srcFilePath, System.IO.FileMode.Open);
            System.IO.FileStream streamWrite = new System.IO.FileStream(dstFilePath, System.IO.FileMode.Create);
            int readLength = 0;
            byte[] buffer = new byte[1024 * 1024 * 10];//1024byte=1KB  10MB
            do
            {
                readLength = streamReader.Read(buffer, 0, buffer.Length);
                streamWrite.Write(buffer, 0, readLength);

            } while (readLength >= buffer.Length);
            streamWrite.Dispose();
            streamReader.Dispose();

        }
        #endregion
        #region 检测文件被占用
        [DllImport("kernel32.dll")]
        private static extern IntPtr _lopen(string lpPathName, int iReadWrite);
        [DllImport("kernel32.dll")]
        private static extern bool CloseHandle(IntPtr hObject);
        private const int OF_READWRITE = 2;
        private const int OF_SHARE_DENY_NONE = 0x40;
        private readonly IntPtr HFILE_ERROR = new IntPtr(-1);

        /// <summary>
        /// 检测文件被占用
        /// </summary>
        /// <param name="FileNames">要检测的文件路径</param>
        /// <returns>返回 false 表示被占用</returns>
        public bool CheckFiles(string FileNames)
        {
            if (!File.Exists(FileNames)) return true; //文件不存在
            
            IntPtr vHandle = _lopen(FileNames, OF_READWRITE | OF_SHARE_DENY_NONE);
            if (vHandle == HFILE_ERROR) return false;//文件被占用
            CloseHandle(vHandle);//文件没被占用
            return true;
        }
        #endregion
        #region 强力粉碎文件，文件如果被打开，很难粉碎
        /// <summary>
        /// 强力粉碎文件，文件如果被打开，很难粉碎
        /// </summary>
        /// <param name="filename">文件全路径</param>
        /// <param name="deleteCount">删除次数</param>
        /// <param name="randomData">随机数据填充文件，默认true</param>
        /// <param name="blanks">空白填充文件，默认false</param>
        /// <returns>true：粉碎成功，false：粉碎失败</returns>
        public static bool KillFile(string filename, int deleteCount, bool randomData = true, bool blanks = false)
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
        #endregion






    }
}
