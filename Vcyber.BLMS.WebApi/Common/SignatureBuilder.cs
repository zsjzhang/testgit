using System;

namespace Vcyber.PublicTools
{
    public class SignatureBuilder
    {
        /// <summary>
        /// 生成安全验证使用的签名
        /// </summary>
        /// <param name="appId">Application Id</param>
        /// <param name="token">约定的token</param>
        /// <param name="timestamp">当前时间戳</param>
        /// <returns>MD5字符串，如果输入为空，则返回空字符串</returns>
        public static string BuildSignature(string appId, string token, string timestamp)
        {
            if (string.IsNullOrEmpty(appId) || string.IsNullOrEmpty(timestamp) || string.IsNullOrEmpty(token)) return string.Empty;
            
            string[] sortArr = { appId.ToLower(), token.ToLower(), timestamp.ToLower() };
            Array.Sort(sortArr);
            string input = string.Join("$", sortArr);
            return Md5Hasher.GetMd5Hash(input);
        }
    }
}
