using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Web;

namespace Vcyber.BLMS.Search
{
    /// <summary>
    /// 搜索结果内容过滤
    /// </summary>
    public class SearchFilter
    {
        private static List<string> filterKeys;

        #region ==== 构造函数 ====

        static SearchFilter()
        {
            filterKeys = new List<string>();
            string filePath = HttpContext.Current.Server.MapPath("~") + "/bin/FilterKey.txt";
            
            FileStream fs = new FileStream(filePath, FileMode.Open,FileAccess.Read);
            StreamReader reader = new StreamReader(fs);
            string key = reader.ReadLine();

            while (!string.IsNullOrEmpty(key))
            {
                filterKeys.Add(key);

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
            foreach (var key in filterKeys)
            {
                content = content.Replace(key, "");
            }

            return content;
        }


        #endregion
    }
}
