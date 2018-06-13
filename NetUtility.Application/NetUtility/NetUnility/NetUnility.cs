using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace NetUtility.NetUnility
{
    public class NetUnility
    {
        /// <summary>
        /// 获取本机的IP地址
        /// </summary>
        /// <returns></returns>
        public static string GetLannIP()
        {
            IPAddress[] addressList = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
            if (addressList.Length < 2)
            {
                return "";
            }
            string IpV4Address = string.Empty;
            for (int i = 0; i < addressList.Length; i++)
            {
                if (addressList[i].AddressFamily == AddressFamily.InterNetwork)
                {
                    IpV4Address = addressList[i].ToString();
                    break;
                }
            }

            return IpV4Address;
        }

        /// <summary>
        /// ip 转换 长整形
        /// </summary>
        /// <param name="strIP"></param>
        /// <returns></returns>
        public static long IP2Long(string strIP)
        {

            long[] ip = new long[4];

            string[] s = strIP.Split('.');
            ip[0] = long.Parse(s[0]);
            ip[1] = long.Parse(s[1]);
            ip[2] = long.Parse(s[2]);
            ip[3] = long.Parse(s[3]);

            return (ip[0] << 24) + (ip[1] << 16) + (ip[2] << 8) + ip[3];
        }

        /// <summary>
        /// 长整形 转换 IP
        /// </summary>
        /// <param name="longIP"></param>
        /// <returns></returns>
        public static string Long2IP(long longIP)
        {


            StringBuilder sb = new StringBuilder("");
            sb.Append(longIP >> 24);
            sb.Append(".");

            //将高8位置0，然后右移16为


            sb.Append((longIP & 0x00FFFFFF) >> 16);
            sb.Append(".");


            sb.Append((longIP & 0x0000FFFF) >> 8);
            sb.Append(".");

            sb.Append((longIP & 0x000000FF));


            return sb.ToString();
        }
 

    }
}
