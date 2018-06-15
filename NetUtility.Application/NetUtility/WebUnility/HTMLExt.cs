using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace NetUtility.WebUnility
{
    public class HTMLExt
    {
        /// <summary>
        /// 获取源代码
        /// </summary>
        /// <param name="url"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string GetPage(string url,Encoding Ec)
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
            catch
            {
                //Main.St.Add(url+"----"+ex.ToString());
                return string.Empty;
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
        public static string GetBody(string htmlString)
        {
            
            Regex reg = new Regex(@"<body[\s\S]*</body>", RegexOptions.IgnoreCase);

            StringBuilder _mcsb = new StringBuilder();
            if (htmlString == null) return string.Empty;
            foreach (Match item in reg.Matches(htmlString))
            {
                _mcsb.AppendLine(item.Value);
            }
            return _mcsb.ToString();


        }
        public static string GetTrLable(string htmlString)
        {
            MatchCollection mc = Regex.Matches(htmlString, @"<tr[\s\S]*</tr>", RegexOptions.IgnoreCase);
            StringBuilder _mcsb = new StringBuilder();
            foreach (Match item in mc)
            {
                _mcsb.AppendLine(item.Value);
            }
            return _mcsb.ToString();
        }
        public static string GetTdLable(string htmlString)
        {
            MatchCollection mc = Regex.Matches(htmlString, @"(?<=<td>)[\s\S]*?(?=</td>)", RegexOptions.IgnoreCase);
            StringBuilder _mcsb = new StringBuilder();
            foreach (Match item in mc)
            {
                _mcsb.AppendLine(item.Value);
            }
            return _mcsb.ToString();
        }

       
       



    }
}
