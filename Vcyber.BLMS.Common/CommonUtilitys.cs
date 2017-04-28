using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Reflection;

namespace Vcyber.BLMS.Common
{
    using System.Configuration;
    using System.Web;
    using System.Xml;
    using System.IO;
    using System.Security.Cryptography;

    /// <summary>
    /// 公共操作类
    /// </summary>
    public static class CommonUtilitys
    {
        #region ==== 公共方法 ====

        /// <summary>
        /// Md5加密
        /// </summary>
        /// <param name="value">加密字符串</param>
        /// <returns></returns>
        public static string EncodeMD5(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new Exception("加密参数不能为空");
            }

            System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] byt, bytHash;
            byt = System.Text.Encoding.UTF8.GetBytes("12" + value);
            bytHash = md5.ComputeHash(byt);
            md5.Clear();
            string sTemp = "";

            for (int i = 0; i < bytHash.Length; i++)
            {
                sTemp += bytHash[i].ToString("x").PadLeft(2, '0');
            }

            return sTemp;
        }
        /// <summary>
        /// MD5
        /// </summary>
        /// <param name="encypStr"></param>
        /// <param name="charset"></param>
        /// <returns></returns>
        public static string EncodeMD5(string encypStr, string charset)
        {
            string retStr;
            MD5CryptoServiceProvider m5 = new MD5CryptoServiceProvider();

            //创建md5对象
            byte[] inputBye;
            byte[] outputBye;

            //使用GB2312编码方式把字符串转化为字节数组．
            try
            {
                inputBye = Encoding.GetEncoding(charset).GetBytes(encypStr);
            }
            catch (Exception ex)
            {
                inputBye = Encoding.GetEncoding("GB2312").GetBytes(encypStr);
            }
            outputBye = m5.ComputeHash(inputBye);

            retStr = System.BitConverter.ToString(outputBye);
            retStr = retStr.Replace("-", "").ToUpper();
            return retStr;
        }
        /// <summary>
        /// 数据拷贝
        /// </summary>
        /// <typeparam name="TSource">被拷贝的数据类型</typeparam>
        /// <typeparam name="TTarget">目标数据类型</typeparam>
        /// <param name="sourceInstance">被拷贝的数据类型实例</param>
        /// <returns>返回目标数据类型</returns>
        public static TTarget CopyData<TSource, TTarget>(TSource sourceInstance)
            where TSource : class,new()
            where TTarget : class,new()
        {
            TTarget targetInstance = new TTarget();

            PropertyInfo[] sourcePros = sourceInstance.GetType().GetProperties();
            PropertyInfo[] targetPros = targetInstance.GetType().GetProperties();

            if (sourcePros == null || targetPros == null)
            {
                throw new Exception("类中不存在公共属性！");
            }

            foreach (var sourceItem in sourcePros)
            {
                foreach (var targetItem in targetPros)
                {
                    if (targetItem.Name.Equals(sourceItem.Name)&&targetItem.GetType()==sourceItem.GetType())
                    {
                        targetItem.SetValue(targetInstance, sourceItem.GetValue(sourceInstance));
                    }
                }
            }

            return targetInstance;
        }

        /// <summary>
        /// 生成编号
        /// </summary>
        /// <returns></returns>
        public static string CreateNumber()
        {
            return System.Math.Abs(Guid.NewGuid().GetHashCode()).ToString();
        }

        public static bool ValidateEmail(string value)
        {
            Regex regex = new Regex("^([0-9A-Za-z\\-_\\.]+)@([0-9a-z]+\\.[a-z]{2,3}(\\.[a-z]{2})?)$");
            return regex.IsMatch(value);
        }

        public static bool ValidatePhone(string value)
        {
            Regex regex = new Regex("^1[0-9]{10}");
            return regex.IsMatch(value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="encodeData"></param>
        /// <returns></returns>
        public static string EncodeBase64(string encodeData)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(encodeData));
        }

        /// <summary>
        /// 解码decodeData
        /// </summary>
        /// <param name="decodeData"></param>
        /// <returns></returns>
        public static string DecodeBase64(string decodeData)
        {
            byte[] bytes = Convert.FromBase64String(decodeData);
            return Encoding.UTF8.GetString(bytes);
        }

        /// <summary>
        /// 根据错误代码获取错误信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string GetMessage(string code)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["MessageFile"]));
            XmlNode node = doc.SelectSingleNode("Messages/message[@code='" + code + "']");
            return node == null ? "未知错误" : node.InnerText;
        }



        public static string GetNikeName()
        {
            string name = Path.GetRandomFileName();
            name = name.Substring(0, name.Length - 4);
            return  "用户"+ name;
        }
        #endregion
    }
}
