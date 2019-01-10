using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace NetUtility.FTP
{
    public class FTPHelper
    {
        private string ip = string.Empty;//FTP Ip地址
        private string userId = string.Empty;//FTP 用户名
        private string password = string.Empty;// FTP 密码

        public FTPHelper(string ip, string userId, string password)
        {
            if (string.IsNullOrEmpty(ip.Trim())) throw new Exception("ip can not be empty");
            this.ip = ip.Trim();
            this.userId = userId;
            this.password = password;
        }
        #region 上传文件到FTP服务器(支持断点上传)
        /// <summary>
        /// 上传文件到FTP服务器(支持断点上传)
        /// </summary>
        /// <param name="localFilePath">本地文件路径</param>
        /// <param name="remoteFilepath">服务器文件路径</param>
        /// <param name="updateProgress">上传进度</param>
        public void FileUpload(string localFilePath, string remoteFilepath, Action<FileProgNum> updateProgress)
        {
            if (string.IsNullOrEmpty(localFilePath)) throw new Exception("local filepath can not be empty");
            if (string.IsNullOrEmpty(remoteFilepath)) throw new Exception("remote filepath can not be empty");
            FileInfo fileInf = new FileInfo(localFilePath);
            //获取本地文件大小
            long allbye = (long)fileInf.Length;
            string newFileName = RemoveSpaces(fileInf.Name);
            //获取服务器文件大小
            long startfilesize = GetFileSize(newFileName, remoteFilepath);
            if (startfilesize >= allbye) return;
            long startbye = startfilesize;
            updateProgress?.Invoke(new FileProgNum() { AllSize = allbye, StartSize = startfilesize });
            string uri;
            if (remoteFilepath.Length == 0)
            {
                uri = "ftp://" + this.ip + @"/" + newFileName;
            }
            else
            {
                //CreateDir("ftp://" + this._FtpServerIP + "/" + remoteFilepath);
                CreateFullDir(remoteFilepath);
                uri = "ftp://" + this.ip + @"/" + remoteFilepath + @"/" + newFileName;
            }
            FtpWebRequest reqFTP;
            // 根据uri创建FtpWebRequest对象 
            reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
            // ftp用户名和密码 
            reqFTP.Credentials = new NetworkCredential(this.userId, this.password);
            // 在一个命令之后被执行   默认为true，连接不会被关闭 
            reqFTP.KeepAlive = false;
            // 指定执行什么命令 
            reqFTP.Method = WebRequestMethods.Ftp.AppendFile;
            // 指定数据传输类型 
            reqFTP.UseBinary = true;
            // 上传文件时通知服务器文件的大小 
            reqFTP.ContentLength = fileInf.Length;
            int buffLength = 2048;// 缓冲大小设置为2kb 
            byte[] buff = new byte[buffLength];
            // 打开一个文件流 (System.IO.FileStream) 去读上传的文件 
            FileStream fs = fileInf.OpenRead();
            Stream strm = null;
            try
            {
                // 把上传的文件写入流 
                strm = reqFTP.GetRequestStream();
                // 每次读文件流的2kb   
                fs.Seek(startfilesize, 0);
                int contentLen = fs.Read(buff, 0, buffLength);
                // 流内容没有结束 
                while (contentLen != 0)
                {
                    // 把内容从file stream 写入 upload stream 
                    strm.Write(buff, 0, contentLen);
                    contentLen = fs.Read(buff, 0, buffLength);
                    startbye += contentLen;
                    //更新进度                    
                    updateProgress?.Invoke(new FileProgNum() { StartSize = startbye, AllSize = allbye });

                }
                // 关闭两个流 
                strm.Close();
                fs.Close();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (fs != null) fs.Close();
                if (strm != null) strm.Close();
            }
        }
        #endregion
        #region 获取远程服务器文件夹下所有的文件名
        /// <summary>
        /// 获取远程服务器文件夹下所有的文件名
        /// </summary>
        /// <param name="dirPath">远程服务器的文件夹路径</param>
        /// <returns></returns>
        public List<string> GetRemoteFiles(string dirPath)
        {
            dirPath = "ftp://" + this.ip + "/" + dirPath;
            List<string> list = new List<string>();
            try
            {
                FtpWebRequest req = (FtpWebRequest)WebRequest.Create(new Uri(dirPath));
                req.Credentials = new NetworkCredential(this.userId, this.password);
                req.Method = WebRequestMethods.Ftp.ListDirectory;
                req.UseBinary = true;
                req.UsePassive = true;
                using (FtpWebResponse res = (FtpWebResponse)req.GetResponse())
                {
                    using (StreamReader sr = new StreamReader(res.GetResponseStream()))
                    {
                        string s;
                        while ((s = sr.ReadLine()) != null)
                        {
                            list.Add(s);
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return list;
        }
        #endregion
        #region 下载远程服务器上的文件
        /// <summary>
        /// 下载远程服务器上的文件
        /// </summary>
        /// <param name="remoteFilePath">远程服务器文件夹路径</param>
        /// <param name="localFilePath">本地路径</param>
        /// <param name="updateProgress">下载进度回调</param>
        public void FileDownLoad(string remoteFilePath, string localFilePath, Action<FileProgNum> updateProgress)
        {
            FtpWebRequest reqFTP, ftpsize;
            Stream ftpStream = null;
            FtpWebResponse response = null;
            FileStream outputStream = null;

            try
            {
                outputStream = new FileStream(localFilePath, FileMode.Append);

                Uri uri = new Uri("ftp://" + this.ip + "/" + remoteFilePath);
                ftpsize = (FtpWebRequest)FtpWebRequest.Create(uri);
                ftpsize.UseBinary = true;
                ftpsize.ContentOffset = 0;

                reqFTP = (FtpWebRequest)FtpWebRequest.Create(uri);
                reqFTP.UseBinary = true;
                reqFTP.KeepAlive = false;
                reqFTP.ContentOffset = 0;

                ftpsize.Credentials = new NetworkCredential(this.userId, this.password);
                reqFTP.Credentials = new NetworkCredential(this.userId, this.password);

                ftpsize.Method = WebRequestMethods.Ftp.GetFileSize;

                FtpWebResponse re = (FtpWebResponse)ftpsize.GetResponse();
                long totalBytes = re.ContentLength;
                re.Close();

                reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
                response = (FtpWebResponse)reqFTP.GetResponse();
                ftpStream = response.GetResponseStream();
                updateProgress?.Invoke(new FileProgNum() { AllSize = totalBytes, StartSize = 0 });
                long totalDownloadedByte = 0;
                int bufferSize = 2048;
                int readCount;
                byte[] buffer = new byte[bufferSize];
                readCount = ftpStream.Read(buffer, 0, bufferSize);
                while (readCount > 0)
                {
                    totalDownloadedByte = readCount + totalDownloadedByte;
                    outputStream.Write(buffer, 0, readCount);
                    //更新进度  
                    updateProgress?.Invoke(new FileProgNum() { AllSize = totalBytes, StartSize = totalDownloadedByte });
                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                }
                ftpStream.Close();
                outputStream.Close();
                response.Close();
            }
            catch (WebException)
            {
                throw;
            }
            finally
            {
                if (ftpStream != null) ftpStream.Close();
                if (outputStream != null) outputStream.Close();
                if (response != null) response.Close();
            }





        }
        #endregion
        #region 去空格
        /// <summary>
        /// 去空格
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static string RemoveSpaces(string str)
        {
            string a = "";
            CharEnumerator CEnumerator = str.GetEnumerator();
            while (CEnumerator.MoveNext())
            {
                byte[] array = new byte[1];
                array = System.Text.Encoding.ASCII.GetBytes(CEnumerator.Current.ToString());
                int asciicode = (short)(array[0]);
                if (asciicode != 32)
                {
                    a += CEnumerator.Current.ToString();
                }
            }
            return a.Split('.')[a.Split('.').Length - 2] + "." + a.Split('.')[a.Split('.').Length - 1];
        }
        #endregion
        #region 获取服务器上文件大小
        /// <summary>
        /// 获取文件大小
        /// </summary>
        /// <param name="filename">文件名称</param>
        /// <param name="remoteFilepath">服务器文件所在的路径</param>
        /// <returns></returns>
        private long GetFileSize(string filename, string remoteFilepath)
        {
            long filesize = 0;
            try
            {
                FtpWebRequest reqFTP;
                FileInfo fi = new FileInfo(filename);
                string uri;
                if (remoteFilepath.Length == 0)
                {
                    uri = "ftp://" + this.ip + "/" + fi.Name;
                }
                else
                {
                    uri = "ftp://" + this.ip + "/" + remoteFilepath + "/" + fi.Name;
                }
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(uri);
                reqFTP.KeepAlive = false;
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(this.userId, this.password);//用户，密码
                reqFTP.Method = WebRequestMethods.Ftp.GetFileSize;
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                filesize = response.ContentLength;
                return filesize;
            }
            catch
            {
                return 0;
            }
        }
        #endregion
        #region 循环在远程服务器上创建文件夹
        /// <summary>
        /// 循环创建文件夹
        /// </summary>
        /// <param name="DirPath"></param>
        public void CreateFullDir(string DirPath)
        {
            string[] arrayDir = DirPath.Split(new char[] { '/' });
            string url = "ftp://" + this.ip;
            for (int i = 0; i < arrayDir.Length; i++)
            {
                url = url + @"/" + arrayDir[i];
                CreateDir(url);
            }
        }
        /// <summary>
        /// 创建文件夹(不循环创建)
        /// </summary>
        /// <param name="uri"></param>
        private void CreateDir(string uri)
        {
            try
            {
                FtpWebRequest reqFTP;
                // 根据uri创建FtpWebRequest对象   
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
                // ftp用户名和密码  
                reqFTP.Credentials = new NetworkCredential(this.userId, this.password);
                // 默认为true，连接不会被关闭  
                // 在一个命令之后被执行  
                reqFTP.KeepAlive = false;
                // 指定执行什么命令  
                reqFTP.Method = WebRequestMethods.Ftp.MakeDirectory;
                // 指定数据传输类型  
                reqFTP.UseBinary = true;
                FtpWebResponse respFTP = (FtpWebResponse)reqFTP.GetResponse();
                respFTP.Close();
            }
            catch (Exception)
            {
                throw;
            }

        }
        #endregion

    }
}
