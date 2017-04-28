using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Search
{
    /// <summary>
    /// 搜索结果
    /// </summary>
    public class SearchResult
    {
        #region ==== 构造函数 ====

        public SearchResult() { }

        #endregion

        #region ==== 公共属性 ====

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// http://www.xxx.com
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 主体内容
        /// </summary>
        public string Content { get; set; }

        #endregion

        #region ==== 公共方法  ====

        public string[] GetPropertyName()
        {
            PropertyInfo[] pros = this.GetType().GetProperties();
            string[] fieldNames = new string[pros.Length];

            for (int i = 0; i < pros.Length; i++)
            {
                PropertyInfo pro = pros[i];
                fieldNames[i] = pro.Name;
            }

            return fieldNames;
        }

        public string[] GetPropertyValue()
        {
            PropertyInfo[] pros = this.GetType().GetProperties();
            string[] texts = new string[pros.Length];

            for (int i = 0; i < pros.Length; i++)
            {
                PropertyInfo pro = pros[i];
                texts[i] = pro.GetValue(this).ToString();
            }

            return texts;
        }

        #endregion
    }
}
