using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vcyber.BLMS.Search;

namespace Vcyber.BLMS.FrontWeb.Controllers
{
    public class SearchController : Controller
    {
        //
        // GET: /Search/
        public ActionResult Index(string key, string index)
        {
            string rootPath = this.Server.MapPath("~");
            string _indexPath = Path.Combine(rootPath, "App_Data\\Index");
            string _xmlPath = Path.Combine(rootPath, "App_Data\\PanGu.xml");
            string _key = string.IsNullOrEmpty(key) ? "第九代索纳塔" : key;
            int _index = 1;
            if (string.IsNullOrEmpty(index) || !int.TryParse(index, out _index) || _index < 1)
            {
                _index = 1;
            }

            int _pagesize = 10;
            int _totalCount = 0;
            SearchEngine _searchEngine = new SearchEngine(_indexPath, _xmlPath);
            IEnumerable<SearchResult> _result = _searchEngine.Search(_key, _index, _pagesize, out _totalCount);
            ViewBag.totalCount = _totalCount;
            ViewBag.pageIndex = _index;
            ViewBag.pageSize = _pagesize;
            ViewBag.pageCount = int.Parse(Math.Ceiling(Convert.ToDouble(_totalCount) / _pagesize).ToString());
            return View(_result);
        }

        public ActionResult SearchContent()
        {
            return View();
        }
    }
}