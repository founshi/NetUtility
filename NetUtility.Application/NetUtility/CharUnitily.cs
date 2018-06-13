using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetUtility
{
    public class CharUnitily
    {
        /// <summary>
        ///字符串转成 char 数组
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static char[] String2CharArray(string str)
        {
            return str.ToCharArray();
        }
        /// <summary>
        /// char数组转成字符串
        /// </summary>
        /// <param name="charArray"></param>
        /// <returns></returns>
        public static string CharArray2String(char[] charArray)
        {
            return new string(charArray);
        }

    }
}
