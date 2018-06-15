using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Runtime.InteropServices;
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

        
    }
}
