using Lucene.Net.Analysis.PanGu;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using PanGu;
using PanGu.HighLight;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Search
{
    /// <summary>
    /// 搜索引擎
    /// </summary>
    public class SearchEngine
    {
        #region ==== 私有字段 ====

        private string indexPath;

        private string xmlPath;

        #endregion

        #region ==== 构造函数 ====

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="indexPath">索引存放路径</param>
        /// <param name="xmlPath">分词配置文件</param>
        public SearchEngine(string indexPath, string xmlPath)
        {
            this.indexPath = indexPath;
            this.xmlPath = xmlPath;
        }

        #endregion

        #region ==== 公共方法 ====

        /// <summary>
        /// 搜索
        /// </summary>
        /// <param name="key">搜索词</param>
        /// <param name="index">分页编号</param>
        /// <param name="size">数据展示个数</param>
        /// <param name="totalCount">搜索总个数</param>
        /// <returns></returns>
        public IEnumerable<SearchResult> Search(string key, int index, int size, out int totalCount)
        {
            List<SearchResult> pageDatas = new List<SearchResult>(5);
            totalCount = 0;

            if (!string.IsNullOrEmpty(key))
            {
                IndexReader reader = null;

                try
                {
                    string[] fieldNames = new SearchResult().GetPropertyName();
                   
                    Hits hits = this.Search(key, this.indexPath, fieldNames, this.xmlPath, out reader);
                    var dataResult = this.ConvertToData(hits, key);
                    totalCount = dataResult.Count;

                    this.PageDataValiate(ref index, ref size, totalCount);

                    for (int i = index; i < size; i++)
                    {
                        pageDatas.Add(dataResult[i]);
                    }
                }
                catch (Exception e) { }
                finally
                {
                    if (reader!=null)
                    {
                         reader.Close();
                    }
                }
            }

            return pageDatas;
        }


        #endregion

        #region ==== 私有方法 ====

        /// <summary>
        /// 结构化数据
        /// </summary>
        /// <param name="hits"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private List<SearchResult> ConvertToData(Hits hits,string key)
        {
            List<SearchResult> pageDatas = new List<SearchResult>(5);
            List<string> contents = new List<string>();

            for (int i = 0; i < hits.Length(); i++)
            {
                SearchResult pageData = new SearchResult();
                Document doc = hits.Doc(i);
                pageData.Title = doc.Get("Title");
                pageData.Url = doc.Get("Url");
                pageData.Content = doc.Get("Content");

                this.FormateResult(pageData, key);

                string tempContent = MD5.GetMD5(pageData.Content);

                if (!contents.Contains(tempContent))
                {
                    pageDatas.Add(pageData);
                    contents.Add(tempContent);
                }
            }

            return pageDatas;
        }

        /// <summary>
        /// 搜索 索引库
        /// </summary>
        /// <param name="text"></param>
        /// <param name="indexPath"></param>
        /// <param name="fieldNames"></param>
        /// <returns></returns>
        private Hits Search(string text, string indexPath, string[] fieldNames, string xmlPath, out IndexReader reader)
        {
            reader = IndexReader.Open(FSDirectory.Open(new DirectoryInfo(indexPath)));
            IndexSearcher search = new IndexSearcher(reader);
            MultiFieldQueryParser par = new MultiFieldQueryParser(fieldNames, new PanGuAnalyzer(xmlPath));
            Query query = par.Parse(text);
            Hits hits = search.Search(query);
            search.Close();

            return hits;
        }

        /// <summary>
        /// 格式化搜索结果
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="key"></param>
        private void FormateResult(SearchResult instance, string key)
        {
            SimpleHTMLFormatter simpleHTMLFormatter = new PanGu.HighLight.SimpleHTMLFormatter("<b style=\"color:red;\">", "</b>");
            Highlighter highlighter = new PanGu.HighLight.Highlighter(simpleHTMLFormatter, new Segment());
            highlighter.FragmentSize = 100;
#warning ==== 先这么写吧 ====
            string title = highlighter.GetBestFragment(key,SearchFilter.ReplaceKey(instance.Title));
            string content = highlighter.GetBestFragment(key, SearchFilter.ReplaceKey(instance.Content));

            instance.Title = string.IsNullOrEmpty(title) ? instance.Title : SearchKeyReplace.ReplaceKey(title);
            instance.Content = string.IsNullOrEmpty(content) ? instance.Content : SearchKeyReplace.ReplaceKey(content);
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <param name="totalCount"></param>
        private void PageDataValiate(ref int index, ref int size, int totalCount)
        {
            index = index <= 0 ? 0 : (index - 1) * size;
            size += index;

            index = index >= totalCount ? 0 : index;
            size = size >= totalCount ? totalCount : size;
        }

        #endregion
    }
}
