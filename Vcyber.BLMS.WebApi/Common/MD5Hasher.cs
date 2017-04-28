using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Vcyber.PublicTools
{
    public class Md5Hasher
    {
        /// <summary>
        /// 获取md5加密字符串(32位)
        /// </summary>
        /// <param name="input">需要加密的字符串</param>
        /// <returns>MD5加密后的字符串</returns>
        public static string GetMd5Hash(string input)
        {
            MD5 md5 = MD5.Create();
            StringBuilder output = new StringBuilder();

            // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　
            byte[] inputbyte = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
            
            // 通过使用循环，将字节类型的数组转换为字符串，此字符串是常规字符格式化所得
            for (int i = 0; i < inputbyte.Length; i++)
            {
                // 将得到的字符串使用十六进制类型格式。格式后的字符是小写的字母，如果使用大写（X）则格式后的字符是大写字符
               output.Append(inputbyte[i].ToString("x2"));
            }
            return output.ToString();
        }
    }
}