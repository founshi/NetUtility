using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

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
        #region 改正sql语句中的转义字符
        /// <summary>
        /// 改正sql语句中的转义字符
        /// </summary>
        public static string mashSQL( this string str)
        {
            string str2;

            if (str == null)
            {
                str2 = "";
            }
            else
            {
                str = str.Replace("\'", "'");
                str2 = str;
            }
            return str2;
        }
        #endregion
        #region 替换sql语句中的有问题符号
        /// <summary>
        /// 替换sql语句中的有问题符号
        /// </summary>
        public static string ChkSQL(string str)
        {
            string str2;

            if (str == null)
            {
                str2 = "";
            }
            else
            {
                str = str.Replace("'", "''");
                str2 = str;
            }
            return str2;
        }
        #endregion
        #region 将字符串转换为Int，转换失败就返回默认值
        /// <summary>
        /// 将字符串转换为Int，转换失败就返回默认值
        /// </summary>
        /// <param name="str"></param>
        /// <param name="defaultVal"></param>
        /// <returns></returns>
        public static int ToInt(this string str, int defaultVal = 0)
        {
            if (string.IsNullOrEmpty(str) || str.Trim().Length >= 11 || !Regex.IsMatch(str.Trim(), @"^([-]|[0-9])[0-9]*(\.\w*)?$"))
                return defaultVal;

            int rv;
            if (Int32.TryParse(str, out rv))
                return rv;

            return Convert.ToInt32(str.ToFloat(defaultVal));

        }
        #endregion
        #region 字符串类型型转换为float型，转换失败就返回默认值
        /// <summary>
        /// 字符串类型型转换为float型，转换失败就返回默认值
        /// </summary>
        /// <param name="str"></param>
        /// <param name="defaultVal"></param>
        /// <returns></returns>
        public static float ToFloat(this string str,float defaultVal=0f)
        {
            if ((str == null) || (str.Length > 10))
                return defaultVal;
            float intValue = defaultVal;
            if (str != null)
            {
                bool IsFloat = Regex.IsMatch(str, @"^([-]|[0-9])[0-9]*(\.\w*)?$");
                if (IsFloat)
                    float.TryParse(str, out intValue);
            }
            return intValue;
        }
        #endregion
        #region 字符串去重
        /// <summary>
        /// 字符串去重
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToDistinctString(this string str)
        {
            if (null == str) return string.Empty;
            if (String.IsNullOrEmpty(str)) return string.Empty;
            return string.Join("", str.ToArray().Distinct().ToArray());
        }
        #endregion
        #region 字符串转换成 bool
        /// <summary>
        /// 字符串转换成 bool
        /// </summary>
        /// <param name="str"></param>
        /// <param name="defaultVal"></param>
        /// <returns></returns>
        public static bool ToBool(this string str, bool defaultVal = false)
        {
            if (str == null) return defaultVal;
            if (string.Compare(str, "true", true) == 0)
                return true;
            else if (string.Compare(str, "false", true) == 0)
                return false;
            return defaultVal;
        }
        #endregion
        #region 格式化字节数字符串
        /// <summary>
        /// 格式化字节数字符串
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string FormatBytesStr(int bytes)
        {
            if (bytes > 1073741824)
            {
                return ((double)(bytes / 1073741824)).ToString("0") + "G";
            }
            if (bytes > 1048576)
            {
                return ((double)(bytes / 1048576)).ToString("0") + "M";
            }
            if (bytes > 1024)
            {
                return ((double)(bytes / 1024)).ToString("0") + "K";
            }
            return bytes.ToString() + "Bytes";
        }
        #endregion
        #region 将全角数字转换为半角数字
        /// <summary>
        /// 将全角数字转换为数字
        /// </summary>
        /// <param name="SBCCase"></param>
        /// <returns></returns>
        public static string SBCCaseToNumberic(string SBCCase)
        {
            char[] c = SBCCase.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                byte[] b = System.Text.Encoding.Unicode.GetBytes(c, i, 1);
                if (b.Length == 2)
                {
                    if (b[1] == 255)
                    {
                        b[0] = (byte)(b[0] + 32);
                        b[1] = 0;
                        c[i] = System.Text.Encoding.Unicode.GetChars(b)[0];
                    }
                }
            }
            return new string(c);
        }
        #endregion
        #region 获取substr第mcount次出现的位置
        /// <summary>
        /// 获取substr第mcount次出现的位置
        /// </summary>
        /// <param name="str">原始字符串</param>
        /// <param name="substr">查找的字符串</param>
        /// <param name="mcount">次数</param>
        /// <returns></returns>
        public static int GetSubStrCountInStr(String str, String substr, int mcount = 2)
        {
            int foundPos = -1;
            int count = 0;
            List<int> foundItems = new List<int>();
            int StartPos = 0;
            do
            {
                foundPos = str.IndexOf(substr, StartPos);
                if (foundPos > -1)
                {
                    StartPos = foundPos + 1;
                    count++;
                    foundItems.Add(foundPos);
                }
            } while (foundPos > -1 && StartPos < str.Length);

            if (mcount > foundItems.Count)
            {
                return foundItems[foundItems.Count - 1];
            }
            else
            {
                return foundItems[mcount - 1];
            }
            //return ((int[])foundItems.ToArray());
        }
        #endregion
        #region 字符串strSrc左边使用padString填充。然后取右边count
        /// <summary>
        /// 字符串strSrc左边使用padString填充。然后取右边count
        /// </summary>
        /// <param name="strSrc">原始字符串</param>
        /// <param name="count">总位数</param>
        /// <param name="padString">左边填充的字符串</param>
        /// <returns></returns>
        public static string ToPadLeft(this string strSrc, int count = 8, string padString = "00000000")
        {
            string mstring = string.Format("{0}{1}", padString, strSrc);
            return mstring.Substring(mstring.Length - 8);
        }
        #endregion


        public static bool IsEmpty(this string str)
        {
            if (str == null) return true;
            return string.IsNullOrEmpty(str);
        }
        public static int Toint(this string strSrc, int defVal = 0)
        {
            strSrc = strSrc.Replace(",", "");
            int.TryParse(strSrc, out defVal);
            return defVal;
        }
        public static double ToDouble(this string strSrc, double defVal = 0)
        {
            double.TryParse(strSrc, out defVal);
            return defVal;
        }

        public static string TrimNull(this string strSrc)
        {
            if (strSrc == null) return "0";
            else if (string.IsNullOrEmpty(strSrc)) return "0";
            else return strSrc.Trim();
        }




    }
}
