using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace NetUtility
{
    public static class StringExt
    {
        #region 字符串转MD5
        /// <summary>
        /// 字符串转MD5
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string MD5(this string str)
        {
            byte[] b = Encoding.UTF8.GetBytes(str);
            b = new MD5CryptoServiceProvider().ComputeHash(b);
            string ret =string.Empty;
            for (int i = 0; i < b.Length; i++)
                ret += b[i].ToString("x").PadLeft(2, '0');
            return ret;
        }
        #endregion





    }
}
