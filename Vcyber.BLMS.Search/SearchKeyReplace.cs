using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Vcyber.BLMS.Search
{
    /// <summary>
    /// 搜索内容替换
    /// </summary>
    internal class SearchKeyReplace
    {
        private static Dictionary<string, string> filterKeys;

        #region ==== 构造函数 ====

        static SearchKeyReplace()
        {
            filterKeys = new Dictionary<string, string>();
            string filePath = HttpContext.Current.Server.MapPath("~") + "/bin/ReplaceKey.txt";

            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            StreamReader reader = new StreamReader(fs);
            string key = reader.ReadLine();

            while (!string.IsNullOrEmpty(key))
            {
                string[] keys = key.Split('=');
                filterKeys.Add(keys[0], keys[1]);

                key = reader.ReadLine();
            }

            reader.Close();
            fs.Close();
        }

        #endregion

        #region ==== 公共方法 ====

        /// <summary>
        /// 替换过滤词
        /// </summary>
        /// <param name="content">过滤内容</param>
        /// <returns></returns>
        public static string ReplaceKey(string content)
        {
            foreach (var key in filterKeys.Keys)
            {
                if (content.StartsWith(key))
                {
                    content = string.Format("{0}{1}", filterKeys[key], content.Substring(key.Length + 1));
                }
            }

            return content;
        }


        #endregion
    }
}
