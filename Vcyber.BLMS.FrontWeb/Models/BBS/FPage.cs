using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Text;

namespace System.Web.Mvc
{
    public static class FPage
    {
        public static IQueryable<T> GetPageList<T>(IOrderedQueryable<T> List, int PageIndex, int PageSize)
        {
            int PageCount = GetPageCount(PageSize, List.Count());
            PageIndex = CheckPageIndex(PageIndex, PageCount);
            return List.Skip((PageIndex - 1) * PageSize).Take(PageSize);
        }

        public static int GetPageCount(int PageSize, int recordCount)
        {
            int PageCount = recordCount % PageSize == 0 ? recordCount / PageSize : recordCount / PageSize + 1;
            if (PageCount < 1) PageCount = 1;
            return PageCount;
        }

        public static int CheckPageIndex(int PageIndex, int PageCount)
        {
            if (PageIndex > PageCount) PageIndex = PageCount;
            if (PageIndex < 1) PageIndex = 1;
            return PageIndex;
        }

        public enum FPageMode { Normal, Numeric, GroupNumeric }
        public static MvcHtmlString ShowFPage(this HtmlHelper Html, string urlFormat, int PageIndex, int PageSize, int recordCount, FPageMode Mode)
        {
            urlFormat = urlFormat.Replace("%7B0%7D", "{0}");
            int PageCount = GetPageCount(PageSize, recordCount);

            StringBuilder TempHtml = new StringBuilder();
            TempHtml.AppendFormat("总共{0}条记录,共{1}页,当前第{2}页&nbsp;&nbsp;", recordCount, PageCount, PageIndex);
            if (PageIndex == 1)
            {
                TempHtml.Append("首页&nbsp;上一页&nbsp;");
            }
            else
            {
                TempHtml.AppendFormat("<a href=\"{0}\">首页</a>&nbsp;", string.Format(urlFormat, 1))
                    .AppendFormat("<a href=\"{0}\">上一页</a>&nbsp;", string.Format(urlFormat, PageIndex - 1));
            }
            // 数字分页
            switch (Mode)
            {
                case FPageMode.Numeric:
                    TempHtml.Append(GetNumericPage(urlFormat, PageIndex, PageSize, PageCount));
                    break;
                case FPageMode.GroupNumeric:
                    TempHtml.Append(GetGroupNumericPage(urlFormat, PageIndex, PageSize, PageCount));
                    break;
            }

            if (PageIndex == PageCount)
            {
                TempHtml.Append("下一页&nbsp;末页");
            }
            else
            {
                TempHtml.AppendFormat("<a href=\"{0}\">下一页</a>&nbsp;", string.Format(urlFormat, PageIndex + 1))
                    .AppendFormat("<a href=\"{0}\">末页</a>", string.Format(urlFormat, PageCount));
            }

            
            
            return MvcHtmlString.Create(TempHtml.ToString());
        }

        /// <summary>
        /// 分组数字分页
        /// </summary>
        /// <param name="urlFormat"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        public static string GetGroupNumericPage(string urlFormat, int pageIndex, int pageSize, int pageCount)
        {
            int GroupChildCount = 10; // 分组显示个数
            int DGroup = pageIndex / GroupChildCount; //当前组
            int GroupCount = pageCount / GroupChildCount;      //组数

            //如果正好是当前组最后一页 不进入下一组
            if (pageIndex % GroupChildCount == 0) DGroup--;

            //当前组数量
            int GroupSpan = (DGroup == GroupCount) ? pageCount % GroupChildCount : GroupChildCount;

            StringBuilder TempHtml = new StringBuilder();
            for (int i = DGroup * GroupChildCount + 1; i <= DGroup * GroupChildCount + GroupSpan; i++)
            {
                if (i == pageIndex)
                    TempHtml.AppendFormat("<span style=\"color:red\">{0}</span>&nbsp;", i);
                else
                    TempHtml.AppendFormat("<a href=\"{0}\">{1}</a>&nbsp;", string.Format(urlFormat, i), i);
            }
            return TempHtml.ToString();
        }

        /// <summary>
        /// 数字分页
        /// </summary>
        /// <param name="urlFormat"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        public static string GetNumericPage(string urlFormat, int pageIndex, int pageSize, int pageCount)
        {
            int SpanNum = 9;
            int BeginNum = pageIndex - (SpanNum - 1) / 2;
            if (BeginNum < 1) BeginNum = 1;
            int EndNum = pageIndex + (SpanNum - 1) / 2;
            if (EndNum > pageCount) EndNum = pageCount;

            StringBuilder TempHtml = new StringBuilder();
            for (int i = BeginNum; i <= EndNum; i++)
            {
                if (i == pageIndex)
                    TempHtml.AppendFormat("<span style=\"color:red\">{0}</span>&nbsp;", i);
                else
                    TempHtml.AppendFormat("<a href=\"{0}\">{1}</a>&nbsp;", string.Format(urlFormat, i), i);
            }
            return TempHtml.ToString();
        }


    }
}