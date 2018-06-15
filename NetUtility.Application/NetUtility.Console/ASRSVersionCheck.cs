using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace NetUtility.Con
{
    /// <summary>
    /// 飞力获取当前ASRS 版本的信息类
    /// </summary>
    public class ASRSVersionCheck
    {
        //@"http://172.20.81.241:8055/publish.htm"
       
        public string HttpString { get; private set; }
        public ASRSVersionCheck(string verSionCheck)
        {
            if (null == verSionCheck) throw new Exception("http不可为空");
            if (string.IsNullOrEmpty(verSionCheck.Trim())) throw new Exception("http不可为空");
            this.HttpString = verSionCheck.Trim();
        }
        private string GetPage(string url, Encoding Ec)
        {
            HttpWebRequest request = null;
            HttpWebResponse webResponse = null;
            try
            {
                request = (HttpWebRequest)WebRequest.Create(url);
                request.AllowAutoRedirect = true;
                request.Timeout = 5000;
                request.ReadWriteTimeout = 5000;
                //request.KeepAlive = true;
                request.Method = "GET";
                //request.ImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Anonymous;
                request.UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)";
                webResponse = (HttpWebResponse)request.GetResponse();
                using (System.IO.Stream stream = webResponse.GetResponseStream())
                {
                    using (System.IO.StreamReader reader2 = new StreamReader(stream, Ec))
                    {
                        return reader2.ReadToEnd();
                    }
                }
            }
            catch//(Exception ex)
            {
                throw;
            }
            finally
            {
                if (request != null)
                {
                    request = null;
                }
                if (webResponse != null)
                {
                    webResponse.Close();
                    webResponse = null;
                }
            }
        }

        private string GetTrLable(string htmlString)
        {
            MatchCollection mc = Regex.Matches(htmlString, @"<tr[\s\S]*</tr>", RegexOptions.IgnoreCase);
            StringBuilder _mcsb = new StringBuilder();
            foreach (Match item in mc)
            {
                _mcsb.AppendLine(item.Value);
            }
            return _mcsb.ToString();
        }
        private string GetTdLable(string htmlString)
        {
            MatchCollection mc = Regex.Matches(htmlString, @"(?<=<td>)[\s\S]*?(?=</td>)", RegexOptions.IgnoreCase);
            StringBuilder _mcsb = new StringBuilder();
            foreach (Match item in mc)
            {
                _mcsb.AppendLine(item.Value);
            }
            return _mcsb.ToString();
        }

        /// <summary>
        /// 获取当前程序版本
        /// </summary>
        /// <returns></returns>
        public string GetCurrentVersion()
        {
            string htmlstring = GetPage(HttpString, Encoding.UTF8);
            int startPostion = 0;
            int EndPostion = 0;
            String Pattern = "<b>名称:</b>";
            MatchCollection Matches = Regex.Matches(htmlstring, Pattern, RegexOptions.IgnoreCase);
            foreach (Match NextMatch in Matches)
            { startPostion = NextMatch.Index; }

            String Pattern1 = "<b>发行者:</b>";
            MatchCollection Matches1 = Regex.Matches(htmlstring, Pattern1, RegexOptions.IgnoreCase);
            foreach (Match NextMatch in Matches1)
            { EndPostion = NextMatch.Index; }


            string _ContainVersion = htmlstring.Substring(startPostion, EndPostion - startPostion);
            _ContainVersion = GetTrLable(_ContainVersion);
            _ContainVersion = GetTdLable(_ContainVersion);

            return _ContainVersion.Replace("<B>版本:</B>\r\n", "").Replace("\r\n", "");

        }




    }
}
