using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Common
{
    public static class CustomMethod
    {
        /// <summary>
        /// 如果传入的值为null、空、空格、0 则返回 string.Empty   
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToEmpty(object obj)
        {
            string strObj = obj + string.Empty;

            if (string.IsNullOrWhiteSpace(strObj) || strObj == "0" || strObj == "0.00")
            {
                return string.Empty;
            }

            return strObj;

        }

        /// <summary>
        /// 传入decimal类型的值，返回一个千位分隔符格式的数字
        /// </summary>
        /// <param name="dec"></param>
        /// <returns></returns>
        public static string DecimalToEmpty(decimal? dec)
        {
            if ((dec ?? 0) == 0)
            {
                return string.Empty;
            }

            return string.Format("{0:N}", dec);
        }
        /// <summary>
        /// 传入decimal类型的值，返回一个千位分隔符格式的数字
        /// </summary>
        /// <param name="dec"></param>
        /// <returns></returns>
        public static string AmountFormat(decimal? dec)
        {
            return string.Format("{0:N}", dec ?? 0);
        }

        /// <summary>
        /// 过滤特殊字符
        /// 如果字符串为空，直接返回。
        /// </summary>
        /// <param name="str">需要过滤的字符串</param>
        /// <returns>过滤好的字符串</returns>
        public static string FilterSpecial(string str)
        {
            if (str == "")
            {
                return str;
            }
            else
            {
                str = str.Replace("'", "");
                str = str.Replace("<", "");
                str = str.Replace(">", "");
                str = str.Replace("%", "");
                str = str.Replace("'delete", "");
                str = str.Replace("''", "");
                str = str.Replace("\"\"", "");
                str = str.Replace(",", "");
                str = str.Replace(".", "");
                str = str.Replace(">=", "");
                str = str.Replace("=<", "");
                str = str.Replace("-", "");
                str = str.Replace("_", "");
                str = str.Replace(";", "");
                str = str.Replace("||", "");
                str = str.Replace("[", "");
                str = str.Replace("]", "");
                str = str.Replace("&", "");
                str = str.Replace("#", "");
                str = str.Replace("/", "");
                str = str.Replace("-", "");
                str = str.Replace("|", "");
                str = str.Replace("?", "");
                str = str.Replace(">?", "");
                str = str.Replace("?<", "");
                str = str.Replace(" ", "");
                return str;
            }
        }
    }
}
