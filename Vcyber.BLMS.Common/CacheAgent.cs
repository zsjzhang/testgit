using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Common
{
    /// <summary>
    /// 数据缓存代理
    /// </summary>
    public class CacheAgent
    {
        #region ==== 私有字段 ====

        private static Dictionary<string, string> datas = new Dictionary<string, string>();

        #endregion

        #region ==== 构造函数 ====

        static CacheAgent() { }

        #endregion

        #region ==== 公共方法 ====

        public static void Write(string keyName, string value)
        {
            lock (datas)
            {
                if (!datas.ContainsKey(keyName))
                {
                    datas.Add(keyName, value);
                }
            }
        }

        public static string Read(string keyName)
        {
            lock (datas)
            {
                if (datas.ContainsKey(keyName))
                {
                    return datas[keyName];
                }

                return string.Empty;
            }
        }

        public static void Remove(string keyName)
        {
            lock (datas)
            {
                datas.Remove(keyName);
            }
        }
        
        #endregion
    }
}
